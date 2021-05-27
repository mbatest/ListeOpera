using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PVS.MediaPlayer;

namespace ListeOpéras
{
    public partial class MainForm : Form
    {
        public static Modèle md = new Modèle();
        public static ImageList imagesMusiciens;
        readonly string BasePath = @"M:\Opéras";
        public static string chrome;
        BackgroundWorker worker;
        SplashScreen spl;
        public static string BOM = Encoding.Unicode.GetString(Encoding.Unicode.GetPreamble());
        public MainForm()
        {
            InitializeComponent();
            //foreach(Marqueurs mk in md.Marqueurs)
            //{
            //    mk.Date = TimeSpan.FromTicks((long)mk.Adresse).ToString();
            //}
            //md.SaveChanges();
            Cursor = Cursors.WaitCursor;
            chrome =  Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Google\Chrome\Application\chrome.exe";
            splitFichiers.Panel2Collapsed = true;
            splitContainer1.Panel2Collapsed = true;
            int length = md.Disques.Sum(c => (int)c.Durée);
            int heures = length / 60;
            int jours = heures / 24;
            Longueur.Text = length.ToString() + " mn soit " + jours.ToString() + " jours " + (heures % 24).ToString() + " heures";
            TreeView.CheckForIllegalCrossThreadCalls = false;
            #region Initialisation Liste d'images
            imagesMusiciens = new ImageList();
            Bitmap imageVide = new Bitmap(imagesMusiciens.ImageSize.Width, imagesMusiciens.ImageSize.Height);
            using (Graphics graph = Graphics.FromImage(imageVide))
            {
                Rectangle ImageSize = new Rectangle(0, 0, imagesMusiciens.ImageSize.Width, imagesMusiciens.ImageSize.Height);
                graph.FillRectangle(Brushes.LightGreen, ImageSize);
            }
            imagesMusiciens.ImageSize = new Size(20, 30);
            imagesMusiciens.Images.Add(imageVide);
            int x = 0;
            foreach(Image im in imageList1.Images)
            {
                imagesMusiciens.Images.Add("a"+(x++).ToString(), im); 
            }
            #endregion
            arbreFichiers.ImageList = imagesMusiciens;
            arbreFichiers.TreeViewNodeSorter = new NodeSorter();
            #region Traitement événement des arbres
            arbreMusiciens.AfterSelect += Arbre_AfterSelect;
            arbreDirection.AfterSelect += Arbre_AfterSelect;
            arbreMise.AfterSelect += Arbre_AfterSelect;
            arbreRoles.AfterSelect += Arbre_AfterSelect;
            arbreFichiers.MouseClick += ArbreFichiers_MouseClick; ;/*Fichiers*/
            arbreMusiciens.MouseClick += Arbre_MouseClick;
            arbreDirection.MouseClick += Arbre_MouseClick;
            arbreMise.MouseClick += Arbre_MouseClick;
            arbreRoles.MouseClick += Arbre_MouseClick;
            arbreOpéras.MouseClick += Arbre_MouseClick;
            player = new Player();
            #endregion
            KeyPreview = true;
            //Init();
            //https://stackoverflow.com/questions/7955663/how-to-build-splash-screen-in-windows-forms-application
            Cursor = Cursors.Default;
        }
        #region Chargement
        private void MainForm_Load(object sender, EventArgs e)
        //     private void Init()
        {
       
            spl = new SplashScreen();
            worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerAsync();
            spl.ShowDialog();
            int nbDVD = md.Disques.Where(d => d.Source == "Achat confirmé" & d.Format == "DVD").Count();
            int nbBD = md.Disques.Where(d => d.Source == "Achat confirmé" & d.Format == "BD").Count();
            int nombreOp = md.Disques.Select(c => c.Opéra.Code_Opéra).Distinct().Count();
            int nbEnr= md.Disques.Where(c => c.URL!=null).Distinct().Count();
            int nbAuteurs = md.Disques.Select(c => c.Opéra.Code_Musicien).Distinct().Count();
            nombreOpéras.Text = nombreOp.ToString() + " opéras de " + nbAuteurs.ToString() + " compositeurs "
                + nbEnr.ToString() + " enregistrements (dont" + nbDVD + " DVDs et " + nbBD + " BDs soit";
            achetés.Text = (nbDVD+nbBD).ToString() + " achats)";
        }
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
        #endregion
        public void VTTtoSRT(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            string newFile = filename.Replace("Verdi - ", "").Replace("concert ", "");
            newFile = newFile.Replace("Verdi - ", "").Replace(" (complet - ST it-eng-fr-de-esp)_(sous-titres_vtt)_fr", "").Replace("Nouveau", @"Nouveau\Avec virgule").Replace("vtt", "srt");
            string line;
            // on saute l'en-tête
            for (int i = 0; i < 3; i++)
                sr.ReadLine();
            int index = 1;
            StreamWriter sw = new StreamWriter(newFile, true, Encoding.GetEncoding(1252));
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.StartsWith("0"))
                {
                    sw.WriteLine(index++);
                    line = line.Replace(".", ",");
                }
                sw.WriteLine(line);
            }
            sr.Close();
            sw.Close();
        }
        #region Réponse aux événements
        private void TraductionSousTitre_Click(object sender, EventArgs e)
        {
            OpenFileDialog opfd = new OpenFileDialog
            {
                Filter = "Sous-titres(*.vtt) |*.vtt|All files (*.*)|*.*"
            };
            if (opfd.ShowDialog()== DialogResult.OK)
            {
                VTTtoSRT(opfd.FileName);
            }
        }
        /// <summary>
        /// Recherche d'un opéra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Titre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (titre.SelectedIndex == 0) return;
            tabControl.SelectedTab = tabOpéras;
            Opéra op = (Opéra)titre.SelectedItem;
            Musicien mus = op.Musicien;
            foreach(TreeNode tn in arbreOpéras.Nodes)
            {
                if(tn.Tag==mus)
                {
                    foreach(TreeNode ttn in tn.Nodes)
                    {
                        if(ttn.Tag==op)
                        { 
                            ttn.ExpandAll();
                            ttn.EnsureVisible();
                            arbreOpéras.SelectedNode = ttn;
                        }
                    }
                }
            }
           
        }
        /// <summary>
        /// Recherche d'un chanteur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chanteurs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chanteurs.SelectedIndex == 0) return;
            Cursor = Cursors.WaitCursor;
            Musicien musicien = (Musicien)chanteurs.SelectedItem;
            if (tabControl.SelectedTab != tabMusiciens)
                tabControl.SelectedTab = tabMusiciens;
            foreach (TreeNode tn in arbreMusiciens.Nodes)
            {
                if (tn.Text == musicien.Instrument.Nom_Instrument)
                {
                    arbreMusiciens.SelectedNode = tn;
                    if (tn.Nodes.Count == 0)
                    {
                        var musi = md.Musicien.Where(m => m.Instrument.Code_Instrument == musicien.Instrument.Code_Instrument & m.Interprétation.Count > 0).OrderBy(m => m.Instrument.Nom_Instrument).ThenBy(m => m.Nom_Musicien).ThenBy(m => m.Prénom_Musicien);
                        foreach (Musicien mus in musi)
                        {
                            arbreMusiciens.SelectedNode.Nodes.Add(mus.CréeNoeudMusicien());
                        }
                    }
                    foreach (TreeNode ttn in tn.Nodes)
                    {
                        if (ttn.Tag== musicien)
                        {
                            ttn.ExpandAll();
                            ttn.EnsureVisible();
                            arbreMusiciens.SelectedNode = ttn;
                            musicien.ActivitésMusicien(ttn);
                            Cursor = Cursors.Default;
                            return;
                        }
                    }
                }
            }
        }
        private void Refresh_Click(object sender, EventArgs e)
        {
            CréeArbreFichiers();
        }
        /// <summary>
        /// Création d'un nouveau disque sans enregistrement associé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Nouveau_Click(object sender, EventArgs e)
        {
            Disques disque = new Disques();
            disque.Metajour();
            //MiseAJourObjet mf = new MiseAJourObjet(d, md);
            //mf.Show();
        }
        private void Hide_Click(object sender, EventArgs e)
        {
            splitFichiers.Panel1Collapsed = hide.Checked;
        }
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
                var instruments = md.Instrument.Where(i => i.Type == "Voix")
                    .OrderBy(i => i.Nom_Instrument);
                foreach (Instrument ins in instruments)
                {
                    TreeNode tn = new TreeNode(ins.Nom_Instrument)
                    {
                        Tag = ins
                    };
                    arbreMusiciens.Nodes.Add(tn);
                }
                #endregion
            }
            if (tabControl.SelectedTab == tabDirection)
            {
                if (arbreDirection.Nodes.Count > 0) return;
                #region Arbre Direction
                arbreDirection.ImageList = imagesMusiciens;
                foreach (Musicien mus in md.Musicien.Where(m => m.Diriger.Count > 0).OrderBy(m => m.Nom_Musicien))
                {
                    arbreDirection.Nodes.Add(mus.CréeNoeudMusicien());
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
                    TreeNode tn = new TreeNode(op.Titre + " " + op.Musicien.Nom_Musicien)
                    {
                        Tag = op
                    };
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
                foreach (Musicien mus in md.Musicien.Where(m => m.MiseEnScene.Count > 0).OrderBy(m => m.Nom_Musicien))
                {
                    arbreMise.Nodes.Add(mus.CréeNoeudMusicien());
                }
                #endregion
            }
        }
        #endregion
        #region Arborescence fichiers
        private void CréeArbreFichiers()
        {
            arbreFichiers.Nodes.Clear();
            #region Parcours de l'arborescence
            SuspendLayout();
            Musicien musicien = new Musicien();
            int nbOpéras = 0;
            int count = 0;
            int total = Directory.GetDirectories(BasePath).Count();
            foreach (string répertoire in Directory.GetDirectories(BasePath).OrderBy(x => x))
            {
                string nom = Path.GetFileNameWithoutExtension(répertoire).Replace(BOM, "");
                string prénom = "";
                if (nom.Contains(", "))
                {
                    prénom = nom.Split(' ')[1]; nom = nom.Split(' ')[0].Replace(",", "");
                }
                var compositeurs = md.Musicien.Where(m => m.Instrument.Nom_Instrument == "Composition" & m.Nom_Musicien.ToUpper() == nom.ToUpper());
                if (compositeurs.Count() == 1)
                {
                    // Le nom correspond au  nom du répertoire et il n'y a pas d'homonyme
                    musicien = compositeurs.First();
                }
                else if (compositeurs.Count() >= 1)
                {
                    #region Plusieurs compositeurs portent le même nom de famille. 

                    compositeurs = compositeurs.Where(c => c.Prénom_Musicien.StartsWith(prénom));
                    foreach (string f in Directory.GetFiles(répertoire))
                    {
                        // On cherche un fichier répertorié dans le répertoire et on obtient le nom du répertoire
                        string fichier = Path.GetFileName(f);
                        var disques = md.Disques.Where(d => d.Fichier == fichier);
                        if (disques.Count() > 0)
                        {
                            if (musicien != disques.First().Opéra.Musicien)
                            {
                                musicien = disques.First().Opéra.Musicien;
                            }
                            break;
                        }
                    }
                    #endregion
                }
                else
                {
                    // On ne peut pas associer un nom de compositeur au répertoire
                    musicien = new Musicien { Nom_Musicien = nom };
                }
                string[] fichiers = Directory.GetFiles(répertoire).Where(f => !f.EndsWith(".srt") | !f.EndsWith(".sub") | !f.EndsWith(".sfk") | !f.EndsWith(".sfk") | !f.EndsWith(".srtold") | !f.EndsWith(".txt") | !f.EndsWith(".smi")).ToArray();
                nbOpéras += fichiers.Count();

                TreeNode noeudMusicien = musicien.CréeNoeudMusicien();
                noeudMusicien.Name = Path.GetFileNameWithoutExtension(répertoire);
                noeudMusicien.Tag = musicien;
                AddNode(arbreFichiers, noeudMusicien);
                count++;
                int percentComplete = (int)((float)count / (float)total * 100);
                if (worker != null) worker.ReportProgress(percentComplete);
            }
            int nbCompositeurs = arbreFichiers.Nodes.Count; titre.Items.Add("<Choisir l'Opéra>");
            md.Opéra.Where(op => op.Disques.Count > 0).OrderBy(c => c.Musicien.Nom_Musicien).ToList().ForEach(c => titre.Items.Add(c));
            chanteurs.Items.Add("<Choisir le Chanteur>");
            md.Musicien.Where(m => m.Instrument.Type == "Voix" & m.Interprétation.Count > 0).OrderBy(c => c.Nom_Musicien).ToList().ForEach(C => chanteurs.Items.Add(C));
            titre.SelectedIndex = 0;
            chanteurs.SelectedIndex = 0;
            //         nombreOpéras.Text += nbCompositeurs.ToString() + " compositeurs et " + nbOpéras.ToString() + " oeuvres numériques";
            ResumeLayout();
            #endregion
        }
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
                     mus.MetàJourImageMusicien( node);
                    //Image im = Image.FromStream(new MemoryStream(mus.Photo));
                    //if (!arbre.ImageList.Images.ContainsKey(mus.Code_Musicien.ToString()))
                    //    arbre.ImageList.Images.Add(mus.Code_Musicien.ToString(), im);
                    //node.ImageKey = mus.Code_Musicien.ToString();
                    //node.SelectedImageKey = node.ImageKey;
                    //im.Dispose();
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
            mus.Nom_Musicien = mus.Nom_Musicien.Replace(BOM, "");
            md.SaveChanges();
            int opera = imagesMusiciens.Images.IndexOfKey("a9");
            List<string> subtitle = new List<string>() { ".srt", ".sfk", ".sub", ".srtold", ".txt", ".smi" };
            if (Directory.Exists((BasePath + @"\" + mus.Nom_Musicien)))// vrai seulement pour les compositeurs
            {

              #region Crée la liste
                var files = Directory.GetFiles((BasePath + @"\" + mus.Nom_Musicien)).Where(c => !subtitle.Contains(Path.GetExtension(c)));
                int n = files.Count();
                tn.Text = mus.ToString() + " : " + n.ToString() + " enregistrement";
                if (n > 1) tn.Text += "s";
                foreach (string fileName in files)
                {
                    #region Lecture d'un répertoire
                    string ext = Path.GetExtension(fileName);
                    if (subtitle.Contains(ext))
                        continue;
                    string fichier = Path.GetFileName(fileName);
                    bool subt = false;
                    if (fileName.EndsWith("mp4"))
                    {
                        string sub = fileName.Replace("mp4", "srt");
                        if (File.Exists(sub))
                        {
                            subt = true;
                        }
                    }
                    var op = md.Disques.Where(d => d.URL == fileName);
                    TreeNode ttn = new TreeNode(fichier, opera, opera);
                    if (op.Count() == 0)
                    {
                        // Fichier non affecté
                        ttn.NodeFont = new Font(arbreOpéras.Font, FontStyle.Bold);
                        ttn.ForeColor = Color.Red;
                        tn.Nodes.Add(ttn);
                    }
                    else
                    {
                        if (subt)
                            ttn.NodeFont = new Font(arbreOpéras.Font, FontStyle.Bold);
                        if (op.Count() > 0)
                        {
                            Disques dis = (Disques)op.First();
                            ttn.Tag = dis;
                        //    dis.FullPath = fileName;
                            dis.URL = fileName;

                            if (String.IsNullOrEmpty(dis.Format))
                                dis.Format = Path.GetExtension(fileName).Replace(".", "");
                            if (fileName.Contains("1280"))
                                dis.Définition =new Définition { Détail = "HD" };
                            if (mus != dis.Opéra.Musicien)
                            {
                                mus = dis.Opéra.Musicien;
                                //       tn.Text = f;//mus.Nom_Musicien + " " + mus.Prénom_Musicien;
                                tn.Tag = mus;
                            }
                            string ff = Path.GetFileName(fileName);
                            if (dis.Pochette != null)
                            {
                                // dis.MetajourPochette(tn.TreeView);

                                string key = "Dis" + dis.Code_Disque.ToString();
                                if (!imagesMusiciens.Images.ContainsKey(key))
                                {
                                    Image im = Image.FromStream(new MemoryStream(dis.Pochette));
                                    imagesMusiciens.Images.Add(key, im);
                                }
                                ttn.ImageKey = key;
                                ttn.SelectedImageKey = key;

                            }
                            dis.Fichier = ff;
                        }
                        else
                        {
                            ttn.Tag = new Disques { Fichier = fichier, URL = fileName };
                        }
                        if (String.IsNullOrEmpty(tn.ImageKey))
                        {
                            if (mus.Photo != null)
                            {
                                mus.MetàJourImageMusicien(arbreFichiers.SelectedNode);
           //                     MetàJourImageMusicien(arbreFichiers, mus);
                                //Image im = Image.FromStream(new MemoryStream(mus.Photo));
                                ////if (!arbre.ImageList.Images.ContainsKey(mus.Code_Musicien.ToString()))
                                ////    arbre.ImageList.Images.Add(mus.Code_Musicien.ToString(), im);
                                //imagesMusiciens.Images.Add(im);
                                //tn.ImageKey = mus.Code_Musicien.ToString();
                                //tn.SelectedImageKey = tn.ImageKey;
                                //im.Dispose();
                            }
                            else
                            {
                                tn.ImageIndex = opera;
                            }
                        }
                        tn.Nodes.Add(ttn);
                    }
                    #endregion
                }
            //    tn.Expand();
                #endregion
            }
            else
            {
                if (mus.Diriger.Count>0)
                {
                    List<Diriger> directions = mus.Diriger.ToList();
                    foreach(Diriger direction in directions)
                    {
                        TreeNode trn = new TreeNode(direction.Disques.Fichier)
                        {
                            Tag = direction.Disques
                        };
                        tn.Nodes.Add(trn);
                    }
                }
    //            
            }
            arbreFichiers.ExpandAll();
        }
        #endregion      
        #region Sélection d'un noeud
        private void ArbreFichiers_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView arbre = (TreeView)sender;
            if (arbre.SelectedNode == null) return;
            var u = md.Disques.Where(c => c.Durée == 0);
            var x = arbre.SelectedNode.Tag;
            arbre.SelectedNode.Nodes.Clear();
            switch (x)
            {
                case null: // Fichier sans disque associé enregistré
                    string path = BasePath + @"\" + ((Musicien)arbre.SelectedNode.Parent.Tag).Nom_Musicien + @"\";
                    player.Play(path + arbre.SelectedNode.Text);
                    int Durée = (int)(player.Media.Duration.TotalMinutes);
                    string Format = Path.GetExtension(arbre.SelectedNode.Text).Replace(".", "").ToUpper();
                    Disques nouveauDisque = new Disques { URL = path + arbre.SelectedNode.Text, Audio = "Stéréo", Fichier = Path.GetFileName(arbre.SelectedNode.Text), Format = Format, Durée = Durée, Source = "Téléchargement" };
                    nouveauDisque.GetDétails(player);
                    player.Stop();
                    if (nouveauDisque.Metajour())
                    {
                        arbre.SelectedNode.Tag = nouveauDisque;
                        arbre.SelectedNode.ForeColor = Color.Black;
                        if (nouveauDisque.Code_Disque == 0)
                            md.SaveChanges();
                    }
                    Visionner(panel, nouveauDisque);
                    break;
                case Musicien musicien:
                    CréeListeOpéras(arbre.SelectedNode);
                    break;
                case Disques disque:
                    if (disque.Opéra != null)
                    {
                        if (arbre.SelectedNode.Parent.Tag == disque)
                        {
                            // Rien de spécial
                        }
                        else
                        {
                            disque.CréeNoeudDisque(arbre.SelectedNode);
                            //      disque.FullPath = BasePath + @"\" + (string)arbre.SelectedNode.Parent.Name + @"\" + disque.Fichier;
                            Visionner(panel, disque);
                            Text = "Opéras : " + Path.GetFileName(disque.Fichier);
                        }
                    }
                    break;
                case Marqueurs marqueur:
                    if (player != null)
                        if (marqueur.Adresse != null)
                            player.Position.FromBegin = new TimeSpan((long)marqueur.Adresse);
                    break;
                case BaseClass baseClass:
                    {
                        if (baseClass.Metajour())
                        {
                            switch (x)
                            {
                                case Interprétation interprétation:
                                    if (interprétation.Musicien.Photo != null)
                                        interprétation.Musicien.MetàJourImageMusicien(arbre.SelectedNode);
                                    break;
                                case Diriger direction:

                                    if (direction.Musicien.Photo != null)
                                        direction.Musicien.MetàJourImageMusicien(arbre.SelectedNode);
                                    break;
                                case MiseEnScene miseenscène:
                                    if (miseenscène.Musicien.Photo != null)
                                        miseenscène.Musicien. MetàJourImageMusicien(arbre.SelectedNode);
                                    break;
                            }
                        }
                    }
                    break;
                case LienYouTube lien:
                    lien.Play();
                    break;
                case ASIN asin:
                    string url = "https://www.amazon.fr/dp/" + asin;
                    Process.Start(chrome, url);
                    break;
            }
        }
        private void ArbreFichiers_MouseClick(object sender, MouseEventArgs e)
        {
            if (arbreFichiers.SelectedNode != null)
            {
                if (e.Button == MouseButtons.Right)
                {

                    CréeMenu(arbreFichiers);
                }
                else arbreFichiers.SelectedNode.Toggle();
            }
        }
        private void ArbreFichiers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (arbreFichiers.SelectedNode.Tag is Disques disque)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = disque.URL
                };
                _ = Process.Start(startInfo);
            }
        }
        #endregion
        #region Arborescence opéras
        private void ArbreOpéras_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (arbreOpéras.SelectedNode == null) return;
            var tag = arbreOpéras.SelectedNode.Tag;
            if (tag is null) return;
            if (tag is Musicien musicien)
            {
                if (musicien.Instrument.Nom_Instrument == "Composition")
                {
                    List<Disques> disque = md.Disques.Where(c => c.Opéra.Musicien.Code_Musicien == musicien.Code_Musicien).ToList();
                    arbreOpéras.SelectedNode.Text += " : " + disque.Count.ToString() + " enregistrement";
                    if (disque.Count > 1)
                        arbreOpéras.SelectedNode.Text += "s";
                }
                //else musicien.CréeNoeudMusicien(arbreOpéras);
            }
            else if (tag is Opéra opera)
            {
                TreeNode tn0 = arbreOpéras.SelectedNode.Parent;
                int nbOp = ((Musicien)arbreOpéras.SelectedNode.Parent.Tag).Opéra.Count;
                if (arbreOpéras.SelectedNode.Nodes.Count >= nbOp)
                {
                    return;
                }
                else
                    opera.CréeNoeudOpéra(arbreOpéras.SelectedNode);
                //#region Noeud Opéra
                ////arbreOpéras.SelectedNode.Nodes.Clear();
                ////if (!string.IsNullOrEmpty(opera.Argument))
                ////{
                ////    TreeNode t = new TreeNode(opera.Argument);
                ////    arbreOpéras.SelectedNode.Nodes.Add(t);
                ////    t.Tag = "Argument";
                ////}
                ////List<Disques> disk = md.Disques.Where(d => d.Opéra.Code_Opéra == opera.Code_Opéra).OrderBy(d => d.Année).ToList();
                ////arbreOpéras.SelectedNode.Text += " " + disk.Count.ToString() + " enregistrement";
                ////if (disk.Count > 1)
                ////    arbreOpéras.SelectedNode.Text += "s";
                ////foreach (Disques dis in disk)
                ////{
                ////    TreeNode trn = CréeNoeudDisque(dis, arbreOpéras);
                ////    trn.Tag = dis;
                ////}
                //#endregion
            }
            else if (tag is Disques disque)
            {
                #region Noeud Disque
                if (disque.Fichier != null)
                {
                    Visionner(panel, disque);
                    Text = "Opéras : " + Path.GetFileName(disque.Fichier);
                }
                arbreOpéras.SelectedNode.Expand();
                #endregion
            }
            else if (tag is LienYouTube lien)
            {
                lien.Play();
            }
            else if (tag is ASIN asin)
            {
                //         if (asin.asin.Length == 10)
                //{
                string url = "https://www.amazon.fr/dp/" + asin;
                Process.Start(chrome, url);
            }
            else if (tag is String argument)
            {
                if (argument == "Argument")
                {
                    Form arg = new Form
                    {
                        Text = arbreOpéras.SelectedNode.Parent.Tag.ToString()
                    };
                    RichTextBox Argument = new RichTextBox
                    {
                        Text = arbreOpéras.SelectedNode.Text
                    };
                    arg.Controls.Add(Argument);
                    Argument.Visible = true;
                    Argument.Dock = DockStyle.Fill;
                    arg.Show();
                }
            } 
            arbreOpéras.SelectedNode.Expand();
        }
        private void CréeArbreOpéras()
        {
            arbreOpéras.Nodes.Clear();
            int opera = imagesMusiciens.Images.IndexOfKey("a9");
            arbreOpéras.ImageList = imagesMusiciens;
            List<Opéra> opéras = md.Opéra.Where(o => o.Disques.Count > 0).OrderBy(o => o.Musicien.Nom_Musicien).ThenBy(o => o.Titre).ToList();
            Musicien musCourant = null;
            TreeNode noeudCourant = null;
            foreach (Opéra op in opéras)
            {
                if (op.Musicien != musCourant)
                {
                    musCourant = op.Musicien;
                    noeudCourant = musCourant.CréeNoeudMusicien();
                    noeudCourant.Tag = musCourant;
                    noeudCourant.Name = musCourant.Nom_Musicien + ", " + musCourant.Prénom_Musicien.Substring(0, 1);
                    arbreOpéras.Nodes.Add(noeudCourant);
                }
                TreeNode tn = new TreeNode(op.Titre, opera, opera)
                {
                    Tag = op
                };
                noeudCourant.Nodes.Add(tn);
            }
        }
        #endregion
        #region Autres arbres
        private void Arbre_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView arbre = (TreeView)sender;
            arbre.SelectedNode.Nodes.Clear();
            switch (arbre.SelectedNode.Tag)
            {
                case Instrument instrument:
                    #region Musiciens associés à un type d'instrument
                    
                    var musi = md.Musicien.Where(m => m.Instrument.Code_Instrument == instrument.Code_Instrument & m.Interprétation.Count > 0).OrderBy(m => m.Instrument.Nom_Instrument).ThenBy(m => m.Nom_Musicien).ThenBy(m => m.Prénom_Musicien);
                    foreach (Musicien m in musi)
                        arbre.SelectedNode.Nodes.Add(m.CréeNoeudMusicien());
                    #endregion
                    break;
                case Musicien musicien:
                    musicien.ActivitésMusicien(arbre.SelectedNode);
                    break;
                case Roles role:
                    foreach (Interprétation interp in role.Interprétation.OrderBy(c => c.Disques.Opéra.Titre))
                    {
                        if (arbre == arbreMusiciens)
                        {
                            if (arbre.SelectedNode.Parent.Tag.ToString() != interp.Musicien.ToString())
                            {
                                TreeNode tni = interp.Musicien.CréeNoeudMusicien();
                                tni.Tag = interp;
                                arbre.SelectedNode.Nodes.Add(tni);
                            }
                            else
                            {
                                interp.Disques.CréeNoeudDisque(arbre.SelectedNode);
                            }
                        }
                    }
                    break;
                case Marqueurs marqueur:
                    {
                        if (marqueur.Adresse != null)
                            player.Position.FromBegin = new TimeSpan((long)marqueur.Adresse);
                    }
                    break;
                case Disques disque:

                    disque.CréeNoeudDisque(arbre.SelectedNode);
                    if (!string.IsNullOrEmpty(disque.URL))
                    {
                        Visionner(panel, disque);
                        Text = "Opéras : " + Path.GetFileName(disque.Fichier);
                    }
                    else { MessageBox.Show("Pas de version numérique"); }
                    break;
                case Opéra opera:
                    foreach (Roles r in opera.Roles)
                    {
                        TreeNode tr = new TreeNode(r.Role + " (" + r.Voix + ")")
                        {
                            Tag = r
                        };
                        arbre.SelectedNode.Nodes.Add(tr);
                        foreach (Interprétation u in r.Interprétation)
                        {
                            tr.Nodes.Add(u.Musicien.CréeNoeudMusicien());
                        }
                    }
                    break;
                case Interprétation interpretation:
                    TreeNode tn = new TreeNode(interpretation.Disques.Editeur.NomEditeur + " " + interpretation.Disques.Année.ToString())
                    {
                        Tag = interpretation.Disques
                    };
                    if (!String.IsNullOrEmpty(interpretation.Disques.Fichier))
                        tn.Text += " " + interpretation.Disques.Fichier;
                    arbre.SelectedNode.Nodes.Add(tn);
                    break;
                case LienYouTube lien:

                    lien.Play();
                    break;
            }
            arbre.SelectedNode.Expand();
        }
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
                    switch (arbre.SelectedNode.Tag)
                    {
                        case Musicien musicien:
                            if (musicien.Metajour())
                            {
                                if (musicien.Photo != null)
                                {
                                    musicien.MetàJourImageMusicien(arbre.SelectedNode/*, musicien*/);
                                }
                            }
                            break;
                        case Opéra opera:
                            opera.Metajour();
                            break;
                        case Disques disque:
                            if (disque.Metajour())
                            {
                                if (disque.Pochette != null)
                                {
                                    Image im = Image.FromStream(new MemoryStream(disque.Pochette));
                                    if (!arbre.ImageList.Images.ContainsKey(disque.Code_Disque.ToString()))
                                        arbre.ImageList.Images.Add(disque.Code_Disque.ToString(), im);
                                    arbre.SelectedNode.ImageKey = disque.Code_Disque.ToString();
                                    arbre.SelectedNode.SelectedImageKey = arbre.SelectedNode.ImageKey;
                                    im.Dispose();
                                }
                            }
                            break;
                        case LienYouTube lien:
                            lien.Play();// Process.Start(chrome, lien.YouTube);
                            break;
                        case ASIN asin:
                            string url = "https://www.amazon.fr/dp/" + asin;
                            Process.Start(chrome, url);
                            break;
                        default:
                            CréeMenu(arbre);
                            break;
                    }
                    #endregion
                }
                else
                {
                    CréeMenu(arbre);
                }
            }
            else arbre.SelectedNode.Toggle();

        }
         #endregion
        #region Méthodes d'usage commun
        private void CréeMenu(TreeView arbre)
        {
            ContextMenu contextMenu = new ContextMenu();
            //if ((arbre.SelectedNode != null) && (arbre.SelectedNode.Tag != null))
            string type = arbre.SelectedNode.Tag.GetType().BaseType.Name;
            if( (type == "Object")|(type=="BaseClass"))
            {
                type = arbre.SelectedNode.Tag.GetType()/*.BaseType*/.Name;
            }
            string[] menus = { "Modifier " + type, "Nouveau" };
            if (arbre.SelectedNode.Tag is Interprétation)
            {
                if (!arbre.SelectedNode.Text.EndsWith("Inconnu"))
                    menus = new string[] { "Modifier Interprétation", "Modifier interprète" };
            }
            foreach (string s in menus)
            {
                MenuItem menuItem = new MenuItem(s);
                menuItem.Click += Menu_Click;
                contextMenu.MenuItems.Add(menuItem);
            }
            Point p = arbre.SelectedNode.Bounds.Location;
            contextMenu.Show(this, p);
            contextMenu.Tag = arbre;
        }
        private void Menu_Click(object sender, EventArgs e)
        {
            TreeView arbre = (TreeView)((MenuItem)sender).Parent.Tag;
            var tag = arbre.SelectedNode.Tag;
            String text = ((MenuItem)sender).Text;
            switch (tag)
            {
                case Musicien musicien:
                    switch (text)
                    {
                        case "Nouveau":
                            musicien = new Musicien();
                            break;
                        default:
                            break;
                    }
                    if (musicien.Metajour())
                    {
                        arbre.SelectedNode.Text = musicien.Nom_Musicien + "  " + musicien.Prénom_Musicien;
                        if (musicien.Photo != null)
                        {
                            musicien.MetàJourImageMusicien(arbre.SelectedNode/*, musicien*/);
                        }
                    }
                    break;
                case MiseEnScene misenScène:
                    if (misenScène.Metajour())
                    {
                        if (misenScène.Musicien != null)
                        {
                            Disques d = (Disques)arbre.SelectedNode.Parent.Tag;
                            d.MiseEnScene.Add(misenScène);
                            arbre.SelectedNode.Text = "Mise en scène : " + misenScène.Musicien.ToString();
                            if (misenScène.Musicien.Photo != null)
                            {
                                misenScène.Musicien.MetàJourImageMusicien(arbre.SelectedNode);

                            }
                        }
                    }
                    break;
                case Marqueurs marqueur:
                    MiseAJourObjet mfe = new MiseAJourObjet(marqueur, arbre.SelectedNode);
                        if (mfe.ShowDialog() == DialogResult.OK)
                        {
                            string[] s = marqueur.Date.Split(':');
                            int h = int.Parse(s[0]);
                            int mn = int.Parse(s[1]);
                            int sec = int.Parse(s[2]);
                            TimeSpan ticks = new TimeSpan(h, mn, sec);
                            marqueur.Adresse = ticks.Ticks;
                            arbre.SelectedNode.Text = marqueur.ToString();
                        }
                    //marqueur.Metajour();
                    break;
                case Orchestres orchestre:
                    orchestre.Metajour();
                    break;
                case Diriger diriger:
                    if (diriger.Metajour())
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
                    break;
                case Interprétation interprétation:
                    switch (text)
                    {
                        case "Modifier Interprétation":
                        case "Modifier interprétation":
                            MiseAJourObjet mfx = new MiseAJourObjet(tag/*, md*/, arbre.SelectedNode);
                            if (mfx.ShowDialog() == DialogResult.OK)
                            {
                                if (interprétation.Musicien != null)
                                {
                                    MetàJourInterprétation(interprétation, arbre);
                                }
                            }
                            break;
                        case "Modifier interprète":
                            mfx = new MiseAJourObjet(interprétation.Musicien/*, md*/, arbre.SelectedNode);
                            if (mfx.ShowDialog() == DialogResult.OK)
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
                        MiseAJourObjet mfo = new MiseAJourObjet(tag/*, md*/);
                        if (mfo.ShowDialog() == DialogResult.OK)
                        {
                            if (interprétation.Musicien != null)
                            {
                                MetàJourInterprétation(interprétation, arbre);
                            }
                        }
                    }
                    break;
                case Disques disque:
                    disque.Fichier = Path.GetFileName(arbre.SelectedNode.Text);
                    disque.Format = Path.GetExtension(disque.Fichier).Replace(".", "").ToUpper();
                    disque.Audio = "Stéréo";
                    if (String.IsNullOrEmpty(disque.Source)) disque.Source = "Téléchargement";
                    disque.Metajour(arbre.SelectedNode);
                    break;
                case Opéra opera:
                    MiseAJourObjet mf = new MiseAJourObjet(opera, arbre.SelectedNode.Parent);
                    if (mf.ShowDialog() == DialogResult.OK)
                    {

                    }
                    break;
                case LienYouTube lien:
                    MiseAJourObjet mfa = new MiseAJourObjet(lien, arbre.SelectedNode.Parent);
                    if (mfa.ShowDialog() == DialogResult.OK)
                    {
                        //          arbre.SelectedNode.Text=
                    }
                    break;
            }
        }
        private void MetàJourInterprétation(Interprétation interprétation, TreeView arbre)
        {
            if (arbre.SelectedNode.Text.EndsWith("Inconnu"))
            {
                if (interprétation.Code_Interprétation == 0)
                    md.Interprétation.Add(interprétation);        
            }
            md.SaveChanges();
            arbre.SelectedNode.Text = interprétation.Roles.Role + " (" + interprétation.Roles.Voix + ")" + " : " + interprétation.Musicien.Nom_Musicien + " " + interprétation.Musicien.Prénom_Musicien + " (" + interprétation.Musicien.Instrument.Nom_Instrument + ")";
            if (interprétation.Musicien.Photo != null)
            {
                interprétation.Musicien.MetàJourImageMusicien(arbre.SelectedNode);
    //            MetàJourImageMusicien(arbre, interprétation.Musicien);
            }
        }
        //private static void MetàJourImageMusicien(TreeView arbre, Musicien musicien)
        //{
        //    MemoryStream ms = new MemoryStream(musicien.Photo);
        //    Image im = Image.FromStream(ms);
        //    if (!arbre.ImageList.Images.ContainsKey(musicien.Code_Musicien.ToString()))
        //        arbre.ImageList.Images.Add(musicien.Code_Musicien.ToString(), im);
        //    arbre.ImageList.Images.Add(musicien.Code_Musicien.ToString(), im);
        //    arbre.SelectedNode.ImageKey = musicien.Code_Musicien.ToString();
        //    arbre.SelectedNode.SelectedImageKey = arbre.SelectedNode.ImageKey;
        //    im.Dispose();
        //}
        #endregion
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (player == null) return true; ;
            Console.WriteLine(keyData);
            switch (keyData)
            {
                case Keys.F8:
                    player.FullScreen = true;
                    break;
                case Keys.F11:
                    player.FullScreen = false;
                    break;
                case Keys.F9:
                    player.Video.Zoom(1.1);              
                    break;
                case Keys.F10:
                    player.Video.Zoom(0.9);
                    break;
                //case Keys.VolumeUp:
                //    break;
                //case Keys.VolumeDown:
                //    break;
                default:
                    Console.WriteLine(keyData);
                    break;
            }
            return false;
            // Call the base class
      //      return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
