using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
//using System.Windows;
using System.Windows.Forms;
using PVS.MediaPlayer;

namespace ListeOpéras
{
    public partial class MainForm : Form
    {
        Modèle md = new Modèle();
        string BasePath = @"M:\Opéras";
        ImageList imagesMusiciens;
        int Confirmé = 0;
        int nbBD;
        int nbCompositeurs;
        int nbOpéras;
        SplashScreen spl;
        public static string BOM = Encoding.Unicode.GetString(Encoding.Unicode.GetPreamble());
        public MainForm()
        {
            InitializeComponent();
            splitContainer1.Panel2Collapsed = true;
            int length = md.Disques.Sum(c => (int)c.Durée);
            int heures = length / 60;
            int jours = heures / 24;
            Longueur.Text = length.ToString() + " mn soit " + jours.ToString() + " jours " + (heures % 24).ToString() + " heures";
            TreeView.CheckForIllegalCrossThreadCalls = false;
            #region Initialisation Liste d'images
            imagesMusiciens = new ImageList();
            imagesMusiciens.ImageSize = new System.Drawing.Size(40, 60);
            Bitmap imageVide = new Bitmap(imagesMusiciens.ImageSize.Width, imagesMusiciens.ImageSize.Height);
            using (Graphics graph = Graphics.FromImage(imageVide))
            {
                Rectangle ImageSize = new Rectangle(0, 0, imagesMusiciens.ImageSize.Width, imagesMusiciens.ImageSize.Height);
                graph.FillRectangle(Brushes.White, ImageSize);
            }
            imagesMusiciens.Images.Add(imageVide);

            imagesMusiciens = new ImageList();
            imagesMusiciens.ImageSize = new Size(40, 60);
            imagesMusiciens.Images.Add(imageVide);
            #endregion

            Confirmé = md.Disques.Where(d => d.Source == "Achat confirmé").Count();
            nbBD = md.Disques.Where(d => d.Source == "Achat confirmé" & d.Format == "BD").Count();
            nombreOpéras.Text = md.Disques.Count().ToString() + " opéras (" + nbBD + " BDs)";
            achetés.Text = Confirmé.ToString() + " achats";
            arbreFichiers.Nodes.Clear();
            arbreFichiers.ImageList = imagesMusiciens;
            #region Traitement événement des arbres
            arbreMusiciens.AfterSelect += Arbre_AfterSelect;
            arbreDirection.AfterSelect += Arbre_AfterSelect;
            arbreMise.AfterSelect += Arbre_AfterSelect;
            arbreRoles.AfterSelect += Arbre_AfterSelect;
            arbreFichiers.MouseClick += Arbre_MouseClick;
            arbreMusiciens.MouseClick += Arbre_MouseClick;
            arbreDirection.MouseClick += Arbre_MouseClick;
            arbreMise.MouseClick += Arbre_MouseClick;
            arbreRoles.MouseClick += Arbre_MouseClick;
            arbreOpéras.MouseClick += Arbre_MouseClick;
            tabControl.TabPages.RemoveAt(tabControl.TabPages.Count - 1);
            player = new Player();
            #endregion

            //https://stackoverflow.com/questions/7955663/how-to-build-splash-screen-in-windows-forms-application
          }
        private void MainForm_Load(object sender, EventArgs e)
        {
            CréeArbreFichiers();
            spl = new SplashScreen();
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerAsync();
            spl.ShowDialog();
        }
        BackgroundWorker worker;
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            spl.ProgressBar.Value = e.ProgressPercentage;
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            spl.Close();
            worker.CancelAsync();
            worker = null;
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            CréeArbreFichiers();
        }
        private void Refresh_Click(object sender, EventArgs e)
        {
            CréeArbreFichiers();
        }
        #region Réponse aux événements
        private void Nouveau_Click(object sender, EventArgs e)
        {
            Disques d = new Disques();
            MiseAJourObjet mf = new MiseAJourObjet(d, md);
            mf.Show();
        }
        #endregion
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!player.Playing) splitContainer1.Panel2Collapsed = true;
            if (tabControl.SelectedTab == tabFichiers)
            {
                if (arbreFichiers.Nodes.Count > 0) return;
                CréeArbreFichiers();
            }
            if (tabControl.SelectedTab == tabOpéras)
            {
                if (arbreOpéras.Nodes.Count > 0) return;
                CréeArbreOpéras();
            }
            if (tabControl.SelectedTab == tabMusiciens)
            {
                if (arbreMusiciens.Nodes.Count > 0) return;
                #region CréeArbreMusiciens
                arbreMusiciens.ImageList = imagesMusiciens;
                arbreMusiciens.Nodes.Clear();
                var instruments = md.Instrument.Where(i => i.Type == "Voix")
                    .OrderBy(i => i.Nom_Instrument);
                foreach (Instrument ins in instruments)
                {
                    TreeNode tn = new TreeNode(ins.Nom_Instrument);
                    tn.Tag = ins;
                    arbreMusiciens.Nodes.Add(tn);
                }
                #endregion
            }
            if (tabControl.SelectedTab == tabDirection)
            {
                if (arbreDirection.Nodes.Count > 0) return;
                #region Arbre Direction
                arbreDirection.ImageList = imagesMusiciens;
                arbreDirection.Nodes.Clear();
                foreach (Musicien mus in md.Musicien.Where(m => m.Diriger.Count > 0).OrderBy(m => m.Nom_Musicien))
                {
                    arbreDirection.Nodes.Add(CréeNoeudMusicien(mus, arbreDirection));
                }
                #endregion
            }
            if (tabControl.SelectedTab == tabRoles)
            {
                if (arbreRoles.Nodes.Count > 0) return;
                #region Arbre Direction
                arbreRoles.ImageList = imagesMusiciens;
                arbreRoles.Nodes.Clear();
                var opéras = md.Opéra.Where(o => o.Disques.Count > 0);
                foreach (Opéra op in opéras)
                {
                    TreeNode tn = new TreeNode(op.Titre + " " + op.Musicien.Nom_Musicien);
                    tn.Tag = op;
                    arbreRoles.Nodes.Add(tn);
                }
                #endregion
            }
            if (tabControl.SelectedTab == tabMise)
            {
                if (arbreMise.Nodes.Count > 0) return;
                #region Mise en Scène
                arbreMise.ImageList = imagesMusiciens;
                arbreMise.Nodes.Clear();
                foreach (Musicien mus in md.Musicien.Where(m => m.MiseEnScene.Count > 0))
                {
                    arbreMise.Nodes.Add(CréeNoeudMusicien(mus, arbreMise));
                }
                #endregion
            }
        }
        #region Arborescence fichiers
        private void CréeArbreFichiers()
        {
            arbreFichiers.Nodes.Clear();
            #region Parcours de l'arborescence
            SuspendLayout();
            Musicien mus = new Musicien();
            nbOpéras = 0;
            int count = 0;
            int total = Directory.GetDirectories(BasePath).Count();
            foreach (string répertoire in Directory.GetDirectories(BasePath).OrderBy(x => x))
            {
                string nom = Path.GetFileNameWithoutExtension(répertoire);
                if ((mus != null) && (mus.Nom_Musicien != répertoire))
                {
                    var x = md.Musicien.Where(m => m.Instrument.Nom_Instrument == "Composition" & m.Nom_Musicien.ToUpper() == nom.ToUpper());
                    if (x.Count() == 1)
                    {
                        // Le nom correspond au  nom du répertoire et il n'y a pas d'homonyme
                        mus = x.First();
                    }
                    else if (x.Count() != 1)
                    {
                        #region Plusieurs compositeurs portent le même nom de famille. 
                        foreach (string f in Directory.GetFiles(répertoire))
                        {
                            // On cherche un fichier répertoiré dans le répertoire et on obtient le nom du répertoire
                            string fichier = Path.GetFileName(f);
                            var disques = md.Disques.Where(d => d.Fichier == fichier);
                            if (disques.Count() > 0)
                            {
                                if (mus != disques.First().Opéra.Musicien)
                                {
                                    mus = disques.First().Opéra.Musicien;
                                }
                                break;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        // On ne peut pas associer un nom de musicien au répertoire
                        mus = new Musicien { Nom_Musicien = nom };
                    }
                    string[] fichiers = Directory.GetFiles(répertoire).Where(f => !f.EndsWith(".srt") | !f.EndsWith(".sfk") | !f.EndsWith(".sfk") | !f.EndsWith(".srtold") | !f.EndsWith(".txt") | !f.EndsWith(".smi")).ToArray();
                    nbOpéras += fichiers.Count();
                }
                if (nom == "Haendel")
                { }
                TreeNode noeudMusicien = CréeNoeudMusicien(mus, arbreFichiers);
                noeudMusicien.Name = nom;
                noeudMusicien.Tag = mus;
                //       arbreFichiers.Nodes.Add(noeudMusicien);
                AddNode(arbreFichiers, noeudMusicien);
                count++;
                int percentComplete = (int)((float)count / (float)total * 100);
                if (worker != null) worker.ReportProgress(percentComplete);
            }
            nbCompositeurs = arbreFichiers.Nodes.Count;
            nombreOpéras.Text = nbOpéras.ToString() + " oeuvres";
            ResumeLayout();
            #endregion
        }
        bool seulInconnus = false;
        public delegate void SetItemCallBack(TreeView ct, TreeNode txt);
        public void AddNode(TreeView arbre, TreeNode node)
        {
            if (this.InvokeRequired)
            {
                SetItemCallBack d = new SetItemCallBack(AddNode);
                this.Invoke(d, new object[] { arbre, node });
            }
            else
            {
                arbre.Nodes.Add(node);
                Musicien mus = (Musicien)node.Tag;
                if (mus.Photo != null)
                {
                    Image im = Image.FromStream(new MemoryStream(mus.Photo));
                    if (!arbre.ImageList.Images.ContainsKey(mus.Code_Musicien.ToString()))
                        arbre.ImageList.Images.Add(mus.Code_Musicien.ToString(), im);
                    node.ImageKey = mus.Code_Musicien.ToString();
                    node.SelectedImageKey = node.ImageKey;
                    im.Dispose();
                }
                else
                {
                    node.ImageIndex = 0;
                    node.StateImageIndex = node.ImageIndex;
                }
            }
        }
        private void CréeListeOpéras(TreeNode tn)
        {
            Musicien mus = (Musicien)tn.Tag;
            string path = BasePath + @"\" + tn.Name;
            if (Directory.Exists(path))// vrai seulement pour les compositeurs
            {
                #region Crée la liste
                foreach (string f in Directory.GetFiles(path))
                {
                    #region Lecture d'un répertoire
                    if (f.EndsWith(".srt") | f.EndsWith(".sfk") | f.EndsWith(".sfk") | f.EndsWith(".srtold") | f.EndsWith(".txt") | f.EndsWith(".smi"))
                        continue;
                    string fichier = Path.GetFileName(f);

                    bool subt = false;
                    if (f.EndsWith("mp4"))
                    {
                        string sub = f.Replace("mp4", "srt");
                        if (File.Exists(sub))
                        {
                            subt = true;
                        }
                    }
                    var op = md.Disques.Where(d => d.Fichier == fichier);
                    if(op.Count()==0)
                    {
                        TreeNode ttn = new TreeNode(f);
                        ttn.NodeFont = new Font(arbreOpéras.Font, FontStyle.Bold);
                        ttn.ForeColor = Color.Red;
                        tn.Nodes.Add(ttn);
                    }
                    else { 
                    TreeNode ttn = new TreeNode(f);
                    if (subt)
                        ttn.NodeFont = new Font(arbreOpéras.Font, FontStyle.Bold);
                        if (seulInconnus)
                        {
                            if (op.Count() == 0)
                            {
                                ttn.Tag = new Disques { Fichier = fichier, FullPath = f };
                                tn.Nodes.Add(ttn);
                            }
                        }
                        else
                        {
                            if (op.Count() > 0)
                            {
                                Disques dis = (Disques)op.First();
                                ttn.Tag = dis;
                                dis.FullPath = f;
                                if (mus != dis.Opéra.Musicien)
                                {
                                    mus = dis.Opéra.Musicien;
                                    //       tn.Text = f;//mus.Nom_Musicien + " " + mus.Prénom_Musicien;
                                    tn.Tag = mus;
                                }
                                string ff = Path.GetFileName(f);
                                if (dis.Pochette != null)
                                {
                                    string key = "Dis" + dis.Code_Disque.ToString();
                                    if (!arbreFichiers.ImageList.Images.ContainsKey(key))
                                    {

                                        Image im = Image.FromStream(new MemoryStream(dis.Pochette));
                                        imagesMusiciens.Images.Add(key, im);
                                    }
                                    ttn.ImageKey = key;
                                    ttn.SelectedImageKey = ttn.ImageKey;

                                }
                                dis.Fichier = ff;
                            }
                            else
                            {
                                ttn.Tag = new Disques { Fichier = fichier, FullPath = f };
                            }
                            if (String.IsNullOrEmpty(tn.ImageKey))
                            {
                                if (mus.Photo != null)
                                {
                                    Image im = Image.FromStream(new MemoryStream(mus.Photo));
                                    arbreFichiers.ImageList.Images.Add(im);
                                    tn.ImageKey = mus.Code_Musicien.ToString();
                                    tn.SelectedImageKey = tn.ImageKey;
                                    im.Dispose();
                                }
                                else
                                {
                                    tn.ImageIndex = 0;
                                }
                            }
                            //if (refreshButton.Checked)
                            //{
                            //    if (((Disques)ttn.Tag).Code_Disque == 0)
                            //        tn.Nodes.Add(ttn);
                            //    tn.Text += " " + tn.Nodes.Count.ToString();
                            //}
                            //else
                            tn.Nodes.Add(ttn);
                        }
                    }
                    #endregion
                }
                tn.Expand();
                #endregion
            }
            arbreFichiers.ExpandAll();
        }
        #region Sélection d'un noeud
        private void ArbreFichiers_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView arbre = (TreeView)sender;
            var u = md.Disques.Where(c => c.Durée == 0);
            if (arbre.SelectedNode != null)
            {
                var x = arbre.SelectedNode.Tag;
                arbre.SelectedNode.Nodes.Clear();
                if (x == null) // Fichier sans disque enregistré 
                {
                    Disques op = new Disques { Fichier = arbre.SelectedNode.Text }; 
                    MiseAJourObjet mf = new MiseAJourObjet(op, md);
                    if(mf.ShowDialog()== DialogResult.OK)
                    {
                        md.Disques.Add(op);
                        md.SaveChanges();
                    }
                }
                if (x is Musicien)
                {
                    Musicien mus = (Musicien)x;
                    arbre.SelectedNode.Nodes.Clear();
                    CréeListeOpéras(arbre.SelectedNode);
                }
                else if (x is Disques dis)
                {
                    if (dis.Opéra != null)
                    {
                        arbre.SelectedNode.Nodes.Clear();
                        CréeNoeudDisque(dis, arbre);
                    }
                    Visionner(((SplitContainer)arbreFichiers.Parent.Parent.Parent.Parent).Panel2, dis);
                    Text = "Opéras : " + Path.GetFileName(dis.Fichier);
                }
                else if (x is Diriger direct)
                {
                }
                else if (x is LienYouTube)
                {

                }
                else if (x is Opéra opera)
                {

                }
                else if (x is Interprétation interp)
                {

                }
            }
        }
        private void ArbreOpéras_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var x = arbreOpéras.SelectedNode.Tag;

            if (x != null)
            {
                if (x is Opéra op)
                {
                    //Opéra op = (Opéra)arbreOpéras.SelectedNode.Tag;
                    #region Noeud Opéra
                    arbreOpéras.SelectedNode.Nodes.Clear();
                    if (!string.IsNullOrEmpty(op.Argument))
                    {
                        TreeNode t = new TreeNode(op.Argument);
                        arbreOpéras.SelectedNode.Nodes.Add(t);
                    }
                    List<Disques> disk = md.Disques.Where(d => d.Opéra.Code_Opéra == op.Code_Opéra).OrderBy(d => d.Année).ToList();
                    foreach (Disques dis in disk)
                    {
                        TreeNode trn = CréeNoeudDisque(dis, arbreOpéras);
                        trn.Tag = dis;
                    }
                    #endregion
                }
                else if (x is Disques dis)
                {
                    #region Noeud Disque
                    //       Disques dis = (Disques)arbreOpéras.SelectedNode.Tag;
                    //                  CréeNoeudDisque(dis, arbreOpéras);

                    if (dis.Source == "Téléchargement")
                    {
                        //          dis.PlayVideo(MediaPlayer, ((SplitContainer)arbreOpéras.Parent.Parent).Panel2);
                        Visionner(splitFichiers.Panel2, dis);
                        Text = "Opéras : " + Path.GetFileName(dis.Fichier);
                    }
                    #endregion
                }
                arbreOpéras.SelectedNode.Expand();
            }
        }
        private void Arbre_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView arbre = (TreeView)sender;
            var x = arbre.SelectedNode.Tag;
            arbre.SelectedNode.Nodes.Clear();
            if (x is Instrument instrum)
            {
                #region Musiciens associés à un type d'instrument
                var musi = md.Musicien.Where(m => m.Instrument.Code_Instrument == instrum.Code_Instrument & m.Interprétation.Count > 0).OrderBy(m => m.Instrument.Nom_Instrument).ThenBy(m => m.Nom_Musicien).ThenBy(m => m.Prénom_Musicien);
                foreach (Musicien m in musi)
                {
                    arbre.SelectedNode.Nodes.Add(CréeNoeudMusicien(m, arbre));
                }
                #endregion
            }
            if (x is Musicien)
            {
                NoeudMusicien(arbre);
            }
            if (x is Roles role)
            {
                foreach (Interprétation interp in role.Interprétation)
                {
                    TreeNode tn = CréeNoeudMusicien(interp.Musicien, arbre);
                    tn.Tag = interp;
                    arbre.SelectedNode.Nodes.Add(tn);
                }
            }
            if (x is Disques disque)
            {
                if (!string.IsNullOrEmpty(disque.Fichier))
                {
                    string path = BasePath + @"\" + disque.Opéra.Musicien.Nom_Musicien.Replace(BOM, "") + @"\" + disque.Fichier.Replace(BOM, "");
                    disque.FullPath = path;
                    Visionner(splitFichiers.Panel2, disque);
                    //   disque.PlayVideo(MediaPlayer, ((SplitContainer)arbre.Parent.Parent).Panel2);
                    Text = "Opéras : " + Path.GetFileName(disque.Fichier);
                }
            }
            if (x is Opéra opera)
            {
                foreach (Roles r in opera.Roles)
                {
                    TreeNode tr = new TreeNode(r.Role + " (" + r.Voix + ")");
                    tr.Tag = r;
                    arbre.SelectedNode.Nodes.Add(tr);
                    foreach (Interprétation u in r.Interprétation)
                    {
                        CréeNoeudMusicien(u.Musicien, arbre);
                    }
                }
            }
            if (x is Interprétation interpretation)
            {
                TreeNode tn = new TreeNode(interpretation.Disques.Editeur.Editeur1 + " " + interpretation.Disques.Année.ToString());
                if (!String.IsNullOrEmpty(interpretation.Disques.Fichier))
                {
                    interpretation.Disques.FullPath = BasePath + interpretation.Disques.Opéra.Musicien.Nom_Musicien + @"\" + interpretation.Disques.Fichier;
                    tn.Text += interpretation.Disques.Fichier;
                }
                tn.Tag = interpretation.Disques;
                arbre.SelectedNode.Nodes.Add(tn);
            }
            arbre.SelectedNode.ExpandAll();
        }
        private void NoeudMusicien(TreeView arbre)
        {
            #region Activités pour un musicien donné
            arbre.SelectedNode.Nodes.Clear();
            Musicien mus = (Musicien)arbre.SelectedNode.Tag;
            var inter = mus.Interprétation.OrderBy(m => m.Roles.Role);
            Roles role = null;
            TreeNode tn = null;
            foreach (Opéra op in mus.Opéra.Where(o => o.Disques.Count > 0).OrderBy(o => o.Année))
            {
                #region Compositions
                TreeNode to = new TreeNode("Composition : " + op.Titre);
                to.Tag = op;
                arbre.SelectedNode.Nodes.Add(to);
                #endregion
            }
            foreach (Diriger dir in mus.Diriger.OrderBy(c => c.Disques.Opéra.Titre))
            {
                #region Chef
                Disques dis = dir.Disques;
                CréeNoeudDisque(dis, arbre);
                #endregion
            }
            foreach (MiseEnScene mise in mus.MiseEnScene)
            {
                #region Metteur en Scène
                Disques dis = mise.Disques;
                CréeNoeudDisque(dis, arbre);
                #endregion
            }
            foreach (Interprétation i in inter)
            {
                #region Interprète
                if (role != i.Roles)
                {
                    role = i.Roles;
                    tn = new TreeNode("Interprète : " + role.Role);
                    tn.Tag = role;
                    arbre.SelectedNode.Nodes.Add(tn);
                }
                TreeNode n = new TreeNode(i.Disques.Fichier);
                if ((i.Disques.Pochette != null) & (i.Disques.ASIN != null))
                {
                    MemoryStream ms = new MemoryStream(i.Disques.Pochette);
                    Image im = Image.FromStream(ms);
                    arbre.ImageList.Images.Add(i.Disques.ASIN.ToString(), im);
                    n.ImageKey = i.Disques.ASIN.ToString();
                    n.SelectedImageKey = n.ImageKey;
                    im.Dispose();
                }
                if (String.IsNullOrEmpty(i.Disques.Fichier))
                {
                    n.Text = i.Disques.Opéra.Titre + " (" + i.Disques.Editeur.Editeur1 + ")";
                }
                tn.Nodes.Add(n);
                n.Tag = i.Disques;
                #endregion
            }
            #endregion
        }
        #endregion
        private void Visionner(Panel panneau, Disques dis)
        {
            string path = BasePath + @"\" + dis.Opéra.Musicien.Nom_Musicien + @"\" + dis.Fichier;
            Init(path, panneau);
            //dis.FullPath = path;
            //panneau.Controls.Clear();
            //foreach (Visionneuse visio in visionneuses)
            //    visio.Dispose();
            //visionneuses.Clear();
            //Visionneuse vis = new Visionneuse();
            //visionneuses.Add(vis);
            //panneau.Controls.Add(vis);
            //vis.Dock = DockStyle.Fill;
            //vis.Init(path);
        }
        #endregion
        #region Arborescence opéras
        private void CréeArbreOpéras()
        {
            arbreOpéras.Nodes.Clear();
            arbreOpéras.ImageList = imagesMusiciens;
            List<Opéra> opéras = md.Opéra.Where(o => o.Disques.Count > 0).OrderBy(o => o.Musicien.Nom_Musicien).ThenBy(o => o.Titre).ToList();
            Musicien musCourant = null;
            TreeNode noeudCourant = null;
            foreach (Opéra op in opéras)
            {
                if (op.Musicien != musCourant)
                {
                    musCourant = op.Musicien;
                    noeudCourant = CréeNoeudMusicien(musCourant, arbreOpéras);
                    noeudCourant.Tag = musCourant;
                    arbreOpéras.Nodes.Add(noeudCourant);
                }
                TreeNode tn = new TreeNode(op.Titre);
                tn.Tag = op;
                noeudCourant.Nodes.Add(tn);
            }
        }
        #endregion
        public TreeNode CréeNoeudMusicien(Musicien mus, TreeView arbre)
        {
            TreeNode n = new TreeNode(mus.Prénom_Musicien + " " + mus.Nom_Musicien);
            n.Tag = mus;
            if (mus.Photo != null)
            {
                Image im = Image.FromStream(new MemoryStream(mus.Photo));
                if (!arbre.ImageList.Images.ContainsKey(mus.Code_Musicien.ToString()))
                    //{
                    //    AddImage(imgsMusiciens, im, mus.Code_Musicien.ToString());
                    //}
                    arbre.ImageList.Images.Add(mus.Code_Musicien.ToString(), im);
                n.ImageKey = mus.Code_Musicien.ToString();
                n.SelectedImageKey = n.ImageKey;
                im.Dispose();
            }
            else
            {
                n.ImageIndex = 0;
                n.StateImageIndex = n.ImageIndex;
            }
            return n;
        }
        private TreeNode CréeNoeudDisque(Disques disque, TreeView arbre)
        {
            #region Noeud Disque
            TreeNode noeudOpéra = new TreeNode(disque.Opéra.Titre + " (" + disque.Opéra.Année?.ToString() + ") "
                + disque.Opéra.Musicien.Nom_Musicien + " " + disque.Opéra.Musicien.Prénom_Musicien);
            if (!String.IsNullOrEmpty(disque.Fichier))
                noeudOpéra.NodeFont = new Font(arbre.Font, FontStyle.Italic);
            if (disque.Pochette != null & disque.ASIN != null)
            {
                string key = "Dis" + disque.Code_Disque.ToString();
                if (!arbre.ImageList.Images.ContainsKey(key))
                {
                    Image im = Image.FromStream(new MemoryStream(disque.Pochette));
                    imagesMusiciens.Images.Add(key, im);
                }
                noeudOpéra.ImageKey = key;
                noeudOpéra.SelectedImageKey = noeudOpéra.ImageKey;
            }
            arbre.SelectedNode.Nodes.Add(noeudOpéra);
            noeudOpéra.Tag = disque;
            TreeNode source = new TreeNode(disque.Année?.ToString() + " " + disque.Source + " " + disque.Editeur?.Editeur1 + " :" + disque.Durée?.ToString() + " mn " + disque.Vidéo + ", " + disque.Audio);
            if (disque.Détail_Définition != null)
                source.Text += ", " + disque.Définition1.Détail;
            noeudOpéra.Nodes.Add(source);
            if (disque.LienYouTube.Count > 0)
            {
                TreeNode th = new TreeNode("YouTube");
                noeudOpéra.Nodes.Add(th);
                foreach (LienYouTube lien in disque.LienYouTube)
                {
                    TreeNode tLien = new TreeNode(lien.YouTube);
                    tLien.Tag = lien;
                    th.Nodes.Add(tLien);
                }
            }
            if (disque.Diriger.Count == 0)
            {
                TreeNode dir = new TreeNode("Inconnu");
                dir.Tag = new Diriger { Disques = disque };
                noeudOpéra.Nodes.Add(dir);
            }
            else
                foreach (Diriger dr in disque.Diriger)
                {
                    #region Orchestre et Direction
                    TreeNode dir = new TreeNode(dr.Orchestres.Nom_Orchestre);
                    dir.Tag = dr;
                    if (dr.Musicien != null)
                    {
                        TreeNode tdir = CréeNoeudMusicien(dr.Musicien, arbre);
                        dir.Nodes.Add(tdir);
                        tdir.Tag = dr.Musicien;
                    }
                    noeudOpéra.Nodes.Add(dir);
                    #endregion
                }
            if (disque.MiseEnScene.Count == 0)
            {
                TreeNode dir = new TreeNode("Metteur en scène inconnu");
                MiseEnScene mise = new MiseEnScene();
                dir.Tag = mise;
                //         d.MiseEnScene.Add(new MiseEnScene { Musicien = mett });
                noeudOpéra.Nodes.Add(dir);
            }
            else
                foreach (MiseEnScene ms in disque.MiseEnScene)
                {
                    #region Mise en scène
                    TreeNode mis = CréeNoeudMusicien(ms.Musicien, arbre);
                    mis.Text = "Mise en scène : " + mis.Text + " " + ms.Salle;
                    noeudOpéra.Nodes.Add(mis);
                    #endregion
                }
            foreach (Roles r in disque.Opéra.Roles.OrderBy(rr => rr.Role))
            {
                #region Interprétations
                TreeNode nn = new TreeNode(r.Role + " (" + r.Voix + ")");
                var u = disque.Interprétation.Where(y => y.Roles == r);
                Interprétation i;
                if (u.Count() > 0)
                {
                    i = u.First();
                    nn.Text += " : " + i.Musicien.Prénom_Musicien + " " + i.Musicien.Nom_Musicien + " (" + i.Musicien.Instrument.Nom_Instrument + ")";
                    Musicien mus = i.Musicien;
                    if (!arbre.ImageList.Images.ContainsKey(mus.Code_Musicien.ToString()))
                    {
                        if (mus.Photo != null)
                        {
                            MemoryStream ms = new MemoryStream(mus.Photo);
                            Image im = Image.FromStream(ms);
                            arbre.ImageList.Images.Add(mus.Code_Musicien.ToString(), im);
                            nn.ImageKey = mus.Code_Musicien.ToString();
                            nn.SelectedImageKey = nn.ImageKey;
                            im.Dispose();
                        }
                    }
                    else
                    {
                        nn.ImageKey = mus.Code_Musicien.ToString();
                        nn.SelectedImageKey = nn.ImageKey;
                    }
                }
                else
                {
                    i = new Interprétation { Roles = r, Disques = disque };
                    nn.Text += " : Inconnu";
                }
                nn.Tag = i;
                noeudOpéra.Nodes.Add(nn);
                #endregion
            }
            arbre.SelectedNode.ExpandAll();
            return noeudOpéra;
            #endregion
        }
        private void Mn_Click(object sender, EventArgs e)
        {
            TreeView arbre = (TreeView)((MenuItem)sender).Parent.Tag;
            var tag = arbre.SelectedNode.Tag;
            String text = ((MenuItem)sender).Text;
            if (tag is Musicien musicien)
            {
                switch (text)
                {
                    case "Nouveau":
                        musicien = new Musicien();
                        break;
                    default:
                        break;
                }
                MiseAJourObjet mf = new MiseAJourObjet(musicien, md, arbre.SelectedNode);
                if (mf.ShowDialog() == DialogResult.OK)
                {
                    arbre.SelectedNode.Text = musicien.Nom_Musicien + "  " + musicien.Prénom_Musicien;
                    if (musicien.Photo != null)
                    {
                        Image im = Image.FromStream(new MemoryStream(musicien.Photo));
                        arbre.ImageList.Images.Add(musicien.Code_Musicien.ToString(), im);
                        arbre.SelectedNode.ImageKey = musicien.Code_Musicien.ToString();
                        arbre.SelectedNode.SelectedImageKey = arbre.SelectedNode.ImageKey;
                        im.Dispose();
                    }
                }
            }
            if (tag is MiseEnScene misenScène)
            {
                MiseAJourObjet mf = new MiseAJourObjet(misenScène, md, arbre.SelectedNode);
                if (mf.ShowDialog() == DialogResult.OK)
                {
                    if (misenScène.Musicien != null)
                    {
                        Disques d = (Disques)arbre.SelectedNode.Parent.Tag;
                        d.MiseEnScene.Add(misenScène);
                        arbre.SelectedNode.Text = "Mise en scène : " + misenScène.Musicien.ToString();
                    }
                }
            }
            if (tag is Orchestres orchestre)
            {

            }
            if (tag is Diriger diriger)
            {
                MiseAJourObjet mf = new MiseAJourObjet(tag, md, arbre.SelectedNode);
                if (mf.ShowDialog() == DialogResult.OK)
                {
                    arbre.SelectedNode.Text = diriger.Orchestres.ToString();
                    arbre.SelectedNode.Nodes.Clear();
                    if (((Diriger)tag).Musicien != null)
                    {
                        TreeNode chef = new TreeNode(diriger.Musicien.ToString())
                        {
                            Tag = diriger.Musicien
                        };
                        arbre.SelectedNode.Nodes.Add(chef);
                    }
                    if (diriger.Code_Disque == 0)
                    {
                        md.Diriger.Add(diriger);
                        md.SaveChanges();
                    }
                }
            }
            if (tag is Interprétation interprétation)
            {
                switch (text)
                {
                    case "Modifier interprétation":
                        MiseAJourObjet mf = new MiseAJourObjet(tag, md, arbre.SelectedNode);
                        if (mf.ShowDialog() == DialogResult.OK)
                        {
                            if (interprétation.Musicien != null)
                            {
                                MetàJourInterprétation(interprétation, arbre);
                            }
                        }
                        break;
                    case "Modifier interprète":
                        mf = new MiseAJourObjet(interprétation.Musicien, md, arbre.SelectedNode);
                        if (mf.ShowDialog() == DialogResult.OK)
                        {
                            if (interprétation.Musicien != null)
                            {
                                MetàJourInterprétation(interprétation, arbre);
                            }
                        }

                        break;
                    default:
                        break;
                }
                if (interprétation.Musicien == null)
                {
                    MiseAJourObjet mf = new MiseAJourObjet(tag, md);
                    if (mf.ShowDialog() == DialogResult.OK)
                    {
                        if (interprétation.Musicien != null)
                        {
                            MetàJourInterprétation(interprétation, arbre);
                        }
                    }
                }
                //else
                //{
                //    MusicienForm mf = new MusicienForm(((Interprétation)x).Musicien, md, arbreFichiers.SelectedNode);
                //    if (mf.ShowDialog() == DialogResult.OK)
                //    {
                //        MetàJourInterprétation((Interprétation)x);
                //    }
                //}
            }
            if (tag is Disques disque)
            {
                disque.Fichier = Path.GetFileName(arbre.SelectedNode.Text);
                disque.Format = Path.GetExtension(disque.Fichier).Replace(".", "").ToUpper();
                disque.Audio = "Stéréo";
                if (String.IsNullOrEmpty(disque.Source)) disque.Source = "Téléchargement";
                MiseAJourObjet mf = new MiseAJourObjet(tag, md, arbre.SelectedNode.Parent);
                if (mf.ShowDialog() == DialogResult.OK)
                {

                }
            }
            if (tag is Opéra opera)
            {
                MiseAJourObjet mf = new MiseAJourObjet(tag, md, arbre.SelectedNode.Parent);
                if (mf.ShowDialog() == DialogResult.OK)
                {

                }

            }
        }
        private void MetàJourInterprétation(Interprétation interprétation, TreeView arbre)
        {
            if (arbre.SelectedNode.Text.EndsWith("Inconnu"))
            {
                md.Interprétation.Add(interprétation);
                md.SaveChanges();
            }
            arbre.SelectedNode.Text = interprétation.Roles.Role + " (" + interprétation.Roles.Voix + ")" + " : " + interprétation.Musicien.Nom_Musicien + " " + interprétation.Musicien.Prénom_Musicien + " (" + interprétation.Musicien.Instrument.Nom_Instrument + ")";
            if (interprétation.Musicien.Photo != null)
            {
                MemoryStream ms = new MemoryStream(interprétation.Musicien.Photo);
                Image im = Image.FromStream(ms);
                arbre.ImageList.Images.Add(interprétation.Musicien.Code_Musicien.ToString(), im);
                arbre.SelectedNode.ImageKey = interprétation.Musicien.Code_Musicien.ToString();
                arbre.SelectedNode.SelectedImageKey = arbreFichiers.SelectedNode.ImageKey;
                im.Dispose();
            }
        }
        //private void arbreFichiers_MouseClick(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        ContextMenu cm = new ContextMenu();
        //        if (arbreFichiers.SelectedNode != null)
        //        {
        //            string type = arbreFichiers.SelectedNode.Tag.GetType().BaseType.Name;
        //            string[] menus = { "Modifier " + type };
        //            if (arbreFichiers.SelectedNode.Tag is Interprétation)
        //            {
        //                if (!arbreFichiers.SelectedNode.Text.EndsWith("Inconnu"))
        //                    menus = new string[] { "Modifier interprétation", "Modifier interprète" };
        //            }
        //            foreach (string s in menus)
        //            {
        //                MenuItem mn = new MenuItem(s);
        //                mn.Click += Mn_Click;
        //                cm.MenuItems.Add(mn);
        //            }
        //            cm.Show(this, arbreFichiers.SelectedNode.Bounds.Location);
        //            var x = arbreFichiers.SelectedNode.Tag;
        //            if (x is Musicien)
        //            {

        //            }

        //        }

        //    }
        //}
        private void Arbre_MouseClick(object sender, MouseEventArgs e)
        {
            TreeView arbre = (TreeView)sender;
            if (arbre.SelectedNode == null) return;
            if (arbre.SelectedNode.Tag == null) return;
            if (e.Button == MouseButtons.Right)
            {
                if (arbre.Name != arbreFichiers.Name)
                {
                    #region Traitement d'un noeud
                    TraiteNoeud(arbre);
                    #endregion
                }
                else
                {
                    CréeMenu(arbre);
                }
            }
        }
        private void TraiteNoeud(TreeView arbre)
        {
            var x = arbre.SelectedNode.Tag;
            if (x is Musicien)
            {
                MiseAJourObjet mf = new MiseAJourObjet(x, md);
                if (mf.ShowDialog() == DialogResult.OK)
                {

                }
            }
            if (x is Opéra oper)
            {

                MiseAJourObjet mf = new MiseAJourObjet(x, md);
                if (mf.ShowDialog() == DialogResult.OK)
                {

                }
            }
            if (x is Disques)
            {

                MiseAJourObjet mf = new MiseAJourObjet(x, md);
                if (mf.ShowDialog() == DialogResult.OK)
                {
                    if (((Disques)x).Pochette != null)
                    {

                    }
                }
            }
            else
            {
                CréeMenu(arbre);
            }
        }
        private void CréeMenu(TreeView arbre)
        {
            ContextMenu cm = new ContextMenu();
            //if ((arbre.SelectedNode != null) && (arbre.SelectedNode.Tag != null))
            string type = arbre.SelectedNode.Tag.GetType().BaseType.Name;
            if (type == "Object")
            {
                type = arbre.SelectedNode.Tag.GetType()/*.BaseType*/.Name;
            }
            string[] menus = { "Modifier " + type, "Nouveau" };
            if (arbre.SelectedNode.Tag is Interprétation)
            {
                if (!arbre.SelectedNode.Text.EndsWith("Inconnu"))
                    menus = new string[] { "Modifier interprétation", "Modifier interprète" };
            }
            foreach (string s in menus)
            {
                MenuItem mn = new MenuItem(s);
                mn.Click += Mn_Click;
                cm.MenuItems.Add(mn);
            }
            Point p = arbre.SelectedNode.Bounds.Location;
            //        p = arbre.PointToClient(p);
            cm.Show(this, p);
            cm.Tag = arbre;
        }
        private void TabControl_Click(object sender, EventArgs e)
        {
            ((TabControl)sender).Select();
        }
        private void ArbreFichiers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (((TreeNode)arbreFichiers.SelectedNode).Tag is Disques)
            {
                Disques d = (Disques)((TreeNode)arbreFichiers.SelectedNode).Tag;
                String path = Path.GetDirectoryName(d.FullPath);
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = d.FullPath;
                Process p = Process.Start(startInfo);
                //Microsoft.DirectX.AudioVideoPlayback.Video vid = new Microsoft.DirectX.AudioVideoPlayback.Video(d.FullPath);
                //vid.Owner = splitFichiers.Panel2;
            }
        }
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            CréeArbreFichiers();
        }


    }
}
