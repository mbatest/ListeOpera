using PVS.MediaPlayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ListeOpéras
{
    #region Classes de base
    public partial class Pays
    {
        public override string ToString()
        {
            return Nom_Pays;
        }
    }
    public partial class Instrument
    {
        public override string ToString()
        {
            return Nom_Instrument;
        }
    }
    public partial class Salles
    {
        public override string ToString()
        {
            return Ville+ " "+ Nom_Salle;
        }
    }
    public partial class Orchestres
    {
        public override string ToString()
        {
            return Nom_Orchestre;
        }
    }
    public partial class Définition
    {
        public override string ToString()
        {
            return Détail;
        }
    }
    public partial class Editeur
    {
        public override string ToString()
        {
            return NomEditeur;
        }
    }

    #endregion
    public abstract class BaseClass
    {
        public bool Metajour()
        {
            MiseAJourObjet mf = new MiseAJourObjet(this);
            if (mf.ShowDialog() == DialogResult.OK)
                return true;
            return false;
        }
        public bool Metajour(TreeNode tn)
        {
            MiseAJourObjet mf = new MiseAJourObjet(this, tn);
            if (mf.ShowDialog() == DialogResult.OK)
            {
                if (this is Marqueurs marqueur)
                {
                    string[] s = marqueur.Date.Split(':');
                    int h = int.Parse(s[0]);
                    int mn = int.Parse(s[1]);
                    int sec = int.Parse(s[2]);
                    TimeSpan ticks = new TimeSpan(h, mn, sec);
                    marqueur.Adresse = ticks.Ticks;
                }
                //else if (this is Musicien musicien)
                //    if(musicien.Photo != null)
                //{
                //    Image im = Image.FromStream(new MemoryStream(musicien.Photo));
                //    MainForm.imagesMusiciens.Images.Add(musicien.Code_Musicien.ToString(), im);
                //    tn.ImageKey = musicien.Code_Musicien.ToString();
                //    tn.SelectedImageKey = tn.ImageKey;
                //    im.Dispose();
                //}

                return true;
            }
            return false;
        }
        //public TreeNode Trouve(TreeView arbre)
        //{
        //    return Trouve(arbre.Nodes); 
        //}
        public TreeNode Trouve(TreeNodeCollection arbre)
        {
            if (arbre.Count == 0) return null;
            foreach(TreeNode tn in arbre)
            {
                //if(tn.Tag.ToString().Contains("Offenbach"))
                //{

                //}
                if (tn.Tag is BaseClass bc)
                {
                    if (bc == this)
                        return tn;
                    else
                        if(tn.Nodes.Count>0) return Trouve(tn.Nodes);
                }
            }
            return null;
        }
    }
    public partial class Marqueurs : BaseClass   {
        public override string ToString()
        {
            if (Airs != null)
                return Date + " " + Airs.Nom;
            else return "";
        }
    }
    public partial class Orchestres : BaseClass { }
    public partial class Diriger:BaseClass
    {
        public override string ToString()
        {
            if (Musicien == null) return "";
            return this.Musicien.ToString() + " " + Orchestres.Nom_Orchestre;
        }
    }
    public partial class Opéra:BaseClass
    {
        public override string ToString()
        {
            return Titre + " (" + Musicien.Nom_Musicien + ", " + Musicien.Prénom_Musicien + " " + Année.ToString() + ")";
        }
        public string Affiche
        {
            get { return ToString(); }
        }
        public TreeNode CréeNoeudOpéra(TreeNode arbre)
        {
            TreeNode tn = arbre;
            int index = MainForm.imagesMusiciens.Images.IndexOfKey("a10") ;
            if (!string.IsNullOrEmpty(Argument))
            {
                TreeNode t = new TreeNode(Argument, index,index);
                tn.Nodes.Add(t);
                t.Tag = "Argument";
            }
            List<Disques> disk = MainForm.md.Disques.Where(d => d.Opéra.Code_Opéra == Code_Opéra).OrderBy(d => d.Année).ToList();
            tn.Text = Titre+ " " + disk.Count.ToString() + " enregistrement";
            if (disk.Count > 1)
                tn.Text += "s";
            foreach (Disques dis in disk)
            {
                dis.CréeNoeudDisque(arbre);
            }
            return tn;
        }
    }
    public partial class Airs
    {
        public override string ToString()
        {
            {
                if (Code_Opéra >0)
                    return Opéra.Titre + " " + Nom;
                else return "";
            }
        }
    }
    public partial class Disques : BaseClass
    {
 //       public string FullPath /*{ get { return @"\" + Opéra.Musicien.Nom_Musicien + @"\" + Fichier; }; set; }*/;

        public override string ToString()
        {
            if (Opéra == null) return "";
            return this.Opéra.Titre;
        }
        public void PlayVideo(Panel panneau, Player player)
        {
            player.Play(URL, panneau);
        }        
        public void GetDétails (Player player)
        {
            Size taille = player.Video.SourceSize;
            string format = taille.Width.ToString() + "x" + taille.Height.ToString();
            float ratio = (float)taille.Width / taille.Height;
            string aspect = "";
            if (ratio == 4 / (float)3) aspect = "4:3";
            if (ratio == 16 / (float)9) aspect = "16:9";
            Vidéo = aspect;
            var def = MainForm.md.Définition.Where(c => c.Détail == format);
            if (def.Count() == 1)
            {
                Définition = def.First();
                Texte_Définition = def.First().Commentaire;
            }
            else
            {
                Définition définition = new Définition { Détail = format };
                MainForm.md.Définition.Add(définition);
                if (taille.Height < 480) définition.Commentaire = "SD";
                if (taille.Width > 1000) définition.Commentaire = "HD";
                Définition = définition;
            }
            MainForm.md.SaveChanges();
        }
        public void CréeNoeudDisque(TreeNode noeud)
        {
         //   TreeNode noeud = arbre.SelectedNode;
            #region Noeud Disque
            int personne = MainForm.imagesMusiciens.Images.IndexOfKey("a0");
            int film = MainForm.imagesMusiciens.Images.IndexOfKey("a1");
            int orchestre = MainForm.imagesMusiciens.Images.IndexOfKey("a2");
            int youtube = MainForm.imagesMusiciens.Images.IndexOfKey("a3");
            int amazon = MainForm.imagesMusiciens.Images.IndexOfKey("a4");
            int marque = MainForm.imagesMusiciens.Images.IndexOfKey("a5");
            int marq = MainForm.imagesMusiciens.Images.IndexOfKey("a6");
            int tv = MainForm.imagesMusiciens.Images.IndexOfKey("a7");
            int dvd = MainForm.imagesMusiciens.Images.IndexOfKey("a8");
            TreeNode noeudOpéra = new TreeNode(Opéra.Titre + " (" + Opéra.Année?.ToString() + ") "
                + Opéra.Musicien.Nom_Musicien + " " + Opéra.Musicien.Prénom_Musicien, film, film)
            {
                Tag = this,
                Name = this.ToString()
            };
            if (!String.IsNullOrEmpty(Fichier))
                noeudOpéra.NodeFont = new Font(noeud.TreeView.Font, FontStyle.Italic);
            if (Pochette != null)
            {
                MetajourPochette(noeud);
            }
            else
            {
                if (MainForm.imagesMusiciens.Images.ContainsKey("a1"))
                {
                    noeudOpéra.ImageKey = "a1";
                    noeudOpéra.SelectedImageKey = noeudOpéra.ImageKey;
                }
            }
            noeudOpéra.Tag = Opéra;
            noeud.Nodes.Add(noeudOpéra);
            TreeNode source = new TreeNode(Année?.ToString() + " " + Source + " " + Editeur?.NomEditeur + " :" + Durée?.ToString() + " mn " + Vidéo + ", " + Audio);
            if (Code_Définition != null)
                source.Text += ", " + Définition.Détail;
            if (Source == "Téléchargement")
                source.ImageIndex = tv;
            else
                source.ImageIndex = dvd;
            noeud.Nodes.Add(source);

            if (!string.IsNullOrEmpty(ASIN))
            {
                TreeNode asin = new TreeNode(ASIN, amazon, amazon)
                {
                    Tag = new ASIN { Asin = ASIN }
                };
                noeud.Nodes.Add(asin);
            }
            if (LienYouTube.Count > 0)
            {
                TreeNode th = new TreeNode("YouTube", youtube, youtube);
                noeud.Nodes.Add(th);
                foreach (LienYouTube lien in LienYouTube)
                {
                    th.Nodes.Add(lien.CréeNoeudLien());
                    th.Tag = lien;
                }
            }
            if (Diriger.Count == 0)     
            {
                TreeNode dir = new TreeNode("Inconnu", orchestre, orchestre)
                {
                    Tag = new Diriger { Disques = this }
                };
                noeud.Nodes.Add(dir);
            }
            else
                foreach (Diriger dr in Diriger)
                {
                    #region Orchestre et Direction
                    TreeNode dir = new TreeNode(dr.Orchestres.Nom_Orchestre, orchestre, orchestre)
                    {
                        Tag = dr
                    };
                    if (dr.Musicien != null)
                    {
                        TreeNode tdir = dr.Musicien.CréeNoeudMusicien();
                        dir.Nodes.Add(tdir);
                        tdir.Tag = dr.Musicien;
                    }
                    noeud.Nodes.Add(dir);
                    #endregion
                }
            if (MiseEnScene.Count == 0)
            {
                TreeNode dir = new TreeNode("Metteur en scène inconnu",personne,personne);
                MiseEnScene mise = new MiseEnScene();
                dir.Tag = mise;
                noeud.Nodes.Add(dir);
            }
            else
                foreach (MiseEnScene ms in MiseEnScene)
                {
                    #region Mise en scène
                    TreeNode mis = ms.Musicien.CréeNoeudMusicien();
                    mis.Tag = ms;
                    mis.Text = "Mise en scène : " + mis.Text + " " + ms.Salle;
                    noeud.Nodes.Add(mis);
                    #endregion
                }
            foreach (Roles r in Opéra.Roles.OrderBy(rr => rr.Role))
            {
                #region Interprétations
                TreeNode nn = new TreeNode(r.Role + " (" + r.Voix + ")");
                var u = Interprétation.Where(y => y.Roles == r);
                Interprétation i;
                if (u.Count() > 0)
                {
                    i = u.First();
                    nn.Text += " : " + i.Musicien.Prénom_Musicien + " " + i.Musicien.Nom_Musicien + " (" + i.Musicien.Instrument.Nom_Instrument + ")";
                    Musicien mus = i.Musicien;
                    if (!MainForm.imagesMusiciens.Images.ContainsKey(mus.Code_Musicien.ToString()))
                    {
                        if (mus.Photo != null)
                        {
                            MemoryStream ms = new MemoryStream(mus.Photo);
                            Image im = Image.FromStream(ms);
                            MainForm.imagesMusiciens.Images.Add(mus.Code_Musicien.ToString(), im);
                            nn.ImageKey = mus.Code_Musicien.ToString();
                            nn.SelectedImageKey = nn.ImageKey;
                            im.Dispose();
                        }
                        else
                        {
                            nn.ImageIndex = personne;
                            nn.SelectedImageKey = nn.ImageKey;

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
                    i = new Interprétation { Roles = r, Disques = this };
                    nn.Text += " : Inconnu";
                    nn.ImageIndex = personne;
                }
                nn.Tag = i;
                noeud.Nodes.Add(nn);
                #endregion
            }
            if (Marqueurs.Count > 0)
            {
                TreeNode marqueurs = new TreeNode("Airs", marque, marque)
                {
                    Tag = "Airs"
                };
                foreach (Marqueurs mar in Marqueurs.OrderBy(c => c.Adresse))
                {
                    TimeSpan time = TimeSpan.FromTicks((long)mar.Adresse);
                    TreeNode marqueur = new TreeNode(mar.ToString(), marq, marq)
                    {
                        Tag = mar
                    };
                    marqueurs.Nodes.Add(marqueur);
                }
                noeud.Nodes.Add(marqueurs);
            }
            noeudOpéra.ExpandAll();
            #endregion

        }
        public void AjoutMarqueur(Marqueurs marqueur, TreeView arbre)
        {
            Marqueurs.Add(marqueur);
            int marqueurImage = MainForm.imagesMusiciens.Images.IndexOfKey("a5");
            int marque = MainForm.imagesMusiciens.Images.IndexOfKey("a6");
            TreeNode tn = Trouve(arbre.Nodes);
            TreeNode marqueurs = null;
            foreach (TreeNode tnr in tn.Nodes)
            {
                if (tnr.Text == "Airs")
                {
                    marqueurs = tnr;

                    break;
                }
            }
            if (marqueurs is null)
            {
                TreeNode[] nodes = arbre.Nodes.Find(this.ToString(), true);

                if (nodes.Length == 0) return;
                marqueurs = new TreeNode("Airs", marqueurImage, marqueurImage)
                {
                    Tag = Marqueurs,
                    Name = "Airs"
                };
                nodes[0].Nodes.Add(marqueurs);
            }
            TreeNode tnMarqueur = new TreeNode(marqueur.ToString(), marque, marque)
            {
                Tag = marqueur
            };
            MainForm.md.Marqueurs.Add(marqueur);
            marqueurs.Nodes.Add(tnMarqueur);
            arbre.SelectedNode.ExpandAll();
            MainForm.md.SaveChanges();
        }
        public void MetajourPochette(TreeNode noeud)
        {
            string key = "Dis" + Code_Disque.ToString();
            Image im = Image.FromStream(new MemoryStream(Pochette));
            if (!MainForm.imagesMusiciens.Images.ContainsKey(key))
                MainForm.imagesMusiciens.Images.Add(key, im);
            noeud.ImageKey = key;
            noeud.SelectedImageKey = noeud.ImageKey;
            im.Dispose();
        }
    }
    public partial class Roles
    {
        public override string ToString()
        {
            return Role + "(" + Voix + ")";
        }
        public void CréeNoeudRole(TreeNode noeud)
        {
            noeud.Nodes.Add(Opéra.CréeNoeudOpéra(noeud));  
        }
    }
    public partial class Musicien : BaseClass
    {
        public override string ToString()
        {
            string text = Nom_Musicien + ", " + Prénom_Musicien;
            if ((Année_Naissance != null) & (Année_Naissance != 0))
                text += " (" + Année_Naissance + " - ";
            if ((Année_Mort != null) & (Année_Mort != 0))
                text += Année_Mort + ")";
            return text;
        }
        public TreeNode CréeNoeudMusicien()
        {
            TreeNode n = new TreeNode(this.ToString());
            int imageIndec = MainForm.imagesMusiciens.Images.IndexOfKey("a1");
            n.Tag = this;
            if (Photo != null)
            {
                Image im = Image.FromStream(new MemoryStream(Photo));
                if (!MainForm.imagesMusiciens.Images.ContainsKey(Code_Musicien.ToString()))
                    MainForm.imagesMusiciens.Images.Add(Code_Musicien.ToString(), im);
                n.ImageKey = Code_Musicien.ToString();
                n.SelectedImageKey = n.ImageKey;
                im.Dispose();
            }
            else
            {
                n.ImageIndex = imageIndec;
                n.StateImageIndex = n.ImageIndex;
            }
            return n;
        }
        public void MetàJourImageMusicien(TreeNode noeud)
        {
            MemoryStream ms = new MemoryStream(Photo);
            Image im = Image.FromStream(ms); 
            if (!noeud.TreeView.ImageList.Images.ContainsKey(Code_Musicien.ToString()))
                noeud.TreeView.ImageList.Images.Add(Code_Musicien.ToString(), im);
            noeud.TreeView.ImageList.Images.Add(Code_Musicien.ToString(), im);
            noeud.ImageKey = Code_Musicien.ToString();
            noeud.SelectedImageKey = noeud.ImageKey;
            im.Dispose();
        }
        public void ActivitésMusicien(TreeNode noeud)
        {
            #region Activités d'un musicien donné
            //TreeNode noeud = arbre.SelectedNode;
            noeud.Nodes.Clear();
            //      Musicien mus = (Musicien)arbre.SelectedNode.Tag;
            //var inter = mus.Interprétation.OrderBy(m => m.Roles.Role);
            Roles role = null;
            TreeNode tn = null;
            foreach (Opéra op in Opéra.Where(o => o.Disques.Count > 0).OrderBy(o => o.Année))
            {
                #region Compositions
                TreeNode to = new TreeNode("Composition : " + op.Titre)
                {
                    Tag = op
                };
                noeud.Nodes.Add(to);
                #endregion
            }
            foreach (Diriger dir in Diriger.OrderBy(c => c.Disques.Opéra.Titre))
            {
                #region Direction
                Disques dis = dir.Disques;
                dis.CréeNoeudDisque(noeud);
                //             CréeNoeudDisque(dis, arbre);
                #endregion
            }
            foreach (MiseEnScene mise in MiseEnScene)
            {
                #region Mise en Scène
                Disques dis = mise.Disques;
                dis.CréeNoeudDisque(noeud);
                //             CréeNoeudDisque(dis, arbre);
                #endregion
            }
            noeud.Text = this.ToString()+" : " + Interprétation.Count + " role";
            if (Interprétation.Count > 1) noeud.Text += "s";

            foreach (Interprétation interprétation in Interprétation.OrderBy(m => m.Roles.Role))
            {
                #region Interprète
                if (role != interprétation.Roles)
                {
                    role = interprétation.Roles;
                    tn = new TreeNode("Interprète : " + role.Role + " (" + role.Opéra.Titre + " - " + role.Opéra.Musicien.ToString() + ")")
                    {
                        Tag = role
                    };
                    //      tn.Nodes.Add(role.Opéra.CréeNoeudOpéra(arbre));
                    noeud.Nodes.Add(tn);
                }
                TreeNode n = new TreeNode(interprétation.Disques.Fichier);
                if ((interprétation.Disques.Pochette != null) & (interprétation.Disques.ASIN != null))
                {
                    MemoryStream ms = new MemoryStream(interprétation.Disques.Pochette);
                    Image im = Image.FromStream(ms);
                    MainForm.imagesMusiciens.Images.Add(interprétation.Disques.ASIN.ToString(), im);
                    n.ImageKey = interprétation.Disques.ASIN.ToString();
                    n.SelectedImageKey = n.ImageKey;
                    im.Dispose();
                }
                if (String.IsNullOrEmpty(interprétation.Disques.Fichier))
                {
                    n.Text = interprétation.Disques.Opéra.Titre + " (" + interprétation.Disques.Editeur.NomEditeur + ")";
                }
                tn.Nodes.Add(n);
                n.Tag = interprétation.Disques;
                #endregion
            }
            #endregion
        }
    }
    public partial class Interprétation  :BaseClass
    {
        public override string ToString()
        {
            if (Disques.Opéra == null) return "";
            return Disques.Opéra.Titre+ " "+ Roles.Role ;
        }
        public TreeNode CréeNoeudInterprétation()
        {
            TreeNode tn = new TreeNode
            {
                Tag = this
            };
            return tn;
        }
        public void Modifier()
        {

        }
    }
    public partial class MiseEnScene:BaseClass
    {
        public override string ToString()
        {
            if (Disques == null) return "";
            return Disques.Opéra.Titre;
        }
        //public TreeNode CréeNoeudMiseEnScene(TreeView arbre)
        //{
        //    TreeNode tn = new TreeNode();
        //    tn.Tag = this;
        //    return tn;
        //}
    }
    public partial class LienYouTube
    {
        public TreeNode CréeNoeudLien()
        {
            int youtube = MainForm.imagesMusiciens.Images.IndexOfKey("a3");
            TreeNode tn = new TreeNode(this.YouTube, youtube, youtube)
            {
                Tag = this
            };
            return tn;
        }
        public void Play()
        {
            if (!String.IsNullOrEmpty(this.YouTube))
                Process.Start(MainForm.chrome, YouTube);
        }
    }
    public class NodeSorter : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = (TreeNode)x;
            TreeNode ty = (TreeNode)y;
            if (tx.Tag is Marqueurs marqueurs)
            {
                if (marqueurs.Adresse < ((Marqueurs)ty.Tag).Adresse)
                    return -1;
                else return 1;
            }
            return 0;
        }
    }
}
