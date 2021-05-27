using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListeOpéras
{
    public partial class MiseAJourObjet : Form
    {
        readonly Object ObjetCourant;
        readonly TreeNode currentNode;
        private static List<Musicien> musiciens;
        //public MiseAJourObjet(object objet, Modèle md) : this(objet, md, null)
        //{

        //}
        public MiseAJourObjet(object objet, TreeNode tn = null)
        {
            if (objet == null)
                return;
            if (tn != null)
                currentNode = tn;
            Cursor = Cursors.WaitCursor;
            InitializeComponent();
            ControlBox = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            //       musiciens = md.Musicien.OrderBy(c => c.Nom_Musicien).ThenBy(c => c.Prénom_Musicien).ToList();
            Type tp = objet.GetType();
            if (tp.FullName.Contains("Proxies"))
                tp = tp.BaseType;
            Text = "Modification " + tp.Name;
            ObjetCourant = objet;
            int y = 30;
            int largeur = 0;
            var mo = tp.GetMethod("ToString")/*.ToList().Where(c => c.Name == "ToString")*/;
            string a = (string)mo.Invoke(objet, null);
            Text += " " + (string)mo.Invoke(objet, null);
            #region Propriétés 
            PropertyInfo[] pis = tp.GetProperties().Where(p => !p.Name.StartsWith("Code_")).ToArray();
            foreach (PropertyInfo propriété in pis)
            {
                Label lb = new Label
                {
                    Text = propriété.Name.Replace("_", " "),
                    Location = new Point(10, y)
                };
                Control edit = null;
                Control edit2 = null;
                Control but = null;
                if (propriété.PropertyType.Name.Contains("IColl"))
                {
                    #region Traitement des collections
                    if (objet is Opéra opera)
                    {
                        #region Cas d'un opéra
                        if (propriété.Name == "Roles")
                        {
                            Controls.Add(lb);
                            DataGridView dtg = new DataGridView
                            {
                                Name = "Roles",
                                AllowUserToAddRows = true,
                                AllowUserToDeleteRows = true
                            };
                            dtg.UserAddedRow += Dtg_UserAddedRow;
                            dtg.RowValidating += Dtg_RowValidating;
                            dtg.RowValidated += Dtg_RowValidated;
                            dtg.UserDeletedRow += Dtg_UserDeletedRow;
                            dtg.Tag = opera;
                            dtg.AutoResizeColumns();
                            var piss = typeof(Roles).GetProperties();
                            foreach (PropertyInfo pi in piss)
                            {
                                if (pi.PropertyType.Name == "String")
                                    dtg.Columns.Add(pi.Name, pi.Name);
                            }
                            if (opera.Roles.Count != 0)
                            {

                                dtg.Rows.Add(opera.Roles.Count);
                                int i = 0;
                                foreach (Roles r in opera.Roles)
                                {
                                    dtg.Rows[i].Tag = r;
                                    foreach (PropertyInfo pi in piss)
                                    {
                                        if (pi.PropertyType.Name == "String")
                                            dtg.Rows[i].Cells[pi.Name].Value = pi
                                                .GetValue(r);
                                    }
                                    i++;
                                }
                            }
                            else
                            {
                                dtg.Rows.Add(1);
                            }
                            dtg.Height = 20 * (dtg.RowCount + 2);
                            edit = dtg;
                        }
                        #endregion
                    }
                    if (objet is Disques disque)
                    {
                        if (propriété.Name == "LienYouTube")
                        {
                            if ((disque.Editeur != null) && ((disque.Editeur.NomEditeur == "You tube") | (disque.LienYouTube.Count != 0)))
                            {
                                Controls.Add(lb);
                                DataGridView dtg = new DataGridView
                                {
                                    Name = "LienYouTube",
                                    AllowUserToAddRows = true,
                                    AllowUserToDeleteRows = true,
                                    Tag = disque
                                };
                                dtg.UserAddedRow += Dtg_UserAddedRow;
                                dtg.RowValidating += Dtg_RowValidating;
                                dtg.RowValidated += Dtg_RowValidated;
                                dtg.UserDeletedRow += Dtg_UserDeletedRow;
                                dtg.AutoResizeColumns();
                                var piss = typeof(LienYouTube).GetProperties();
                                foreach (PropertyInfo pi in piss)
                                {
                                    if (pi.PropertyType.Name == "String")
                                        dtg.Columns.Add(pi.Name, pi.Name);
                                }
                                if (disque.LienYouTube.Count != 0)
                                {

                                    dtg.Rows.Add(disque.LienYouTube.Count);
                                    int i = 0;
                                    foreach (LienYouTube r in disque.LienYouTube)
                                    {
                                        dtg.Rows[i].Tag = r;
                                        foreach (PropertyInfo pi in piss)
                                        {
                                            if (pi.PropertyType.Name == "String")
                                            {
                                                dtg.Rows[i].Cells[pi.Name].Value = pi
                                                    .GetValue(r);
                                            }
                                        }
                                        i++;
                                    }
                                }
                                else
                                {
                                    dtg.Rows.Add(1);
                                    //       dtg.Rows[0].Cells.Add(new Cell);

                                }
                                //                   dtg.Columns[0].Width = ((string) dtg.Rows[0].Cells[0].Value).Length * 6/* ((LienYouTube)disque.LienYouTube.First()).YouTube.Length*5; */;
                                dtg.Height = 20 * (dtg.RowCount + 2);
                                dtg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                                edit = dtg;
                            }
                        }
                    }
                    #endregion
                }
                else if ((propriété.SetMethod != null) & !propriété.Name.Contains("Code"))
                {
                    #region Traitement des propriétes simples
                    switch (propriété.Name)
                    {
                        #region En fonction du nom de propriété
                        case "Musicien":
                            #region Musicien
                            edit = new ComboBox
                            {
                                Name = propriété.Name,
                                Width = 180
                            };
                            //if (ObjetCourant is Opéra)
                            //    musiciens = MainForm.md.Musicien.Where(c => c.Instrument.Nom_Instrument == "Composition").OrderBy(c => c.Nom_Musicien).ToList();
                            //                           if (ObjetCourant.GetType().Name.Contains("Disques") | ObjetCourant.GetType().Name.Contains("Interprétation") | ObjetCourant.GetType().Name.Contains("Diriger") | ObjetCourant.GetType().Name.Contains("MiseEnScene"))
                            //{
                            //    musiciens = MainForm.md.Musicien.Where(c => c.Instrument.Type == "Voix" | c.Instrument.Nom_Instrument == "Direction" | c.Instrument.Nom_Instrument == "Metteur En Scène").OrderBy(c => c.Nom_Musicien).ToList();
                            //}
                            switch (ObjetCourant)
                            {
                                case Opéra opera:
                                    musiciens = MainForm.md.Musicien.Where(c => c.Instrument.Nom_Instrument == "Composition").OrderBy(c => c.Nom_Musicien).ToList();
                                    break;
                                case Disques disque:
                                case Interprétation inter:
                                case Diriger dir:
                                case MiseEnScene mise:
                                    musiciens = MainForm.md.Musicien.Where(c => c.Instrument.Type == "Voix" | c.Instrument.Nom_Instrument == "Direction"
                                    | c.Instrument.Nom_Instrument == "Metteur En Scène").OrderBy(c => c.Nom_Musicien).ToList();
                                    break;
                            }
                            musiciens.ForEach(x => ((ComboBox)edit).Items.Add(x));
                            ((ComboBox)edit).SelectedItem = (Musicien)propriété.GetValue(objet);
                            #endregion
                            but = CréeBouton(propriété);
                            break;
                        case "Opéra":
                            edit = CréeCombo(objet, propriété, tn);
                            but = CréeBouton(propriété);
                            break;
                        case "Editeur":
                        case "Instrument":
                        case "Orchestres":
                        case "Pays":
                        case "Airs":
                            edit = CréeCombo(objet, propriété);
                            but = CréeBouton(propriété);
                            break;
                        case "Salles":
                            edit = CréeCombo(objet, propriété);
                            but = CréeBouton(propriété);
                            break;
                        case "Ville":
                        case "Vidéo":
                        case "Audio":
                        case "Définition":
                            edit = CréeCombo(objet, propriété);
                            break;
                        case "Détail_Définition":
                        case "Format":
                        case "Source":
                            edit = CréeCombo(objet, propriété);
                            break;
                        case "Roles":
                            #region Role
                            edit = new ComboBox
                            {
                                Width = 180,
                                Name = propriété.Name
                            };
                            int op = ((Interprétation)objet).Disques.Opéra.Code_Opéra;
                            foreach (Roles pay in MainForm.md.Roles.Where(x => x.Opéra.Code_Opéra == op).OrderBy(c => c.Role))
                            {
                                ((ComboBox)edit).Items.Add(pay);
                            }
                            Roles curRole = (Roles)propriété.GetValue(objet);
                            ((ComboBox)edit).SelectedItem = curRole;

                            but = new Button
                            {
                                Text = propriété.Name,
                                Tag = propriété
                            };
                            but.Click += But_Click;
                            #endregion
                            break;
                        case "Disques":
                            if (objet is Marqueurs marq)
                                break;
     //                       else { edit = CréeCombo(((Disques)marq.Disques).Opéra, md, propriété); }
                            edit = CréeCombo(objet, propriété);
                            break;
                        #endregion
                        default:
                            #region En fonction directe des types de données
                            switch (propriété.PropertyType.Name)
                            {
                                case "Boolean":
                                    edit = new CheckBox();
                                    break;
                                case "Single":
                                case "Double":
                                    #region Nombre
                                    edit = new NumericUpDown
                                    {
                                        Width = 50,
                                        Maximum = 1000,
                                        DecimalPlaces = 2
                                    };
                                    if (propriété.GetValue(objet) != null)
                                        ((NumericUpDown)edit).Text = propriété.GetValue(objet).ToString();
                                    #endregion
                                    break;
                                case "Point":
                                    #region Point
                                    edit = new NumericUpDown();
                                    ((NumericUpDown)edit).Minimum = -3000;
                                    ((NumericUpDown)edit).Maximum = 3000;
                                    if (propriété.GetValue(objet) != null)
                                        ((NumericUpDown)edit).Value = ((Point)propriété.GetValue(objet)).X;
                                    edit2 = new NumericUpDown();
                                    ((NumericUpDown)edit2).Minimum = -3000;
                                    ((NumericUpDown)edit2).Maximum = 3000;
                                    if (propriété.GetValue(objet) != null)
                                        ((NumericUpDown)edit2).Value = ((Point)propriété.GetValue(objet)).Y;
                                    #endregion
                                    break;
                                case "Size":
                                    #region Taille
                                    edit = new NumericUpDown();
                                    ((NumericUpDown)edit).Minimum = 0;
                                    ((NumericUpDown)edit).Maximum = Screen.PrimaryScreen.Bounds.Width;
                                    if (propriété.GetValue(objet) != null)
                                        ((NumericUpDown)edit).Value = ((Size)propriété.GetValue(objet)).Width;

                                    edit.Width = 50;
                                    edit2 = new NumericUpDown();
                                    ((NumericUpDown)edit2).Minimum = 0;
                                    ((NumericUpDown)edit2).Maximum = Screen.PrimaryScreen.Bounds.Height;
                                    if (propriété.GetValue(objet) != null)
                                        ((NumericUpDown)edit2).Value = ((Size)propriété.GetValue(objet)).Height;

                                    edit2.Width = 50;
                                    break;
                                #endregion
                                case "String":
                                    #region Chaines
                                    edit = new TextBox
                                    {
                                        Name = propriété.Name
                                    };
                                    TextBox Editeur = (TextBox)edit;
                                    if (propriété.GetValue(objet) != null)
                                    {
                                        edit.Text = (String)propriété.GetValue(objet);
                                        if (edit.Text.Length > 100)
                                        {
                                            edit.Size = new Size(400, 100);
                                            ((TextBox)edit).Multiline = true;
                                            ((TextBox)edit).ScrollBars = ScrollBars.Both;
                                        }
                                        if ((edit.Text.Length > 20) & (edit.Text.Length < 100))
                                        {
                                            edit.Size = new Size(300, 20);
                                            ((TextBox)edit).ScrollBars = ScrollBars.Horizontal;
                                        }
                                    }
                                    StringLengthAttribute strLenAttr = propriété.GetCustomAttributes(typeof(StringLengthAttribute), false).Cast<StringLengthAttribute>().SingleOrDefault();
                                    if(strLenAttr== null) // Seul Argument n'a pas de longueur maximale définie
                                    {
                                        Editeur.Multiline = true;
                                        Editeur.ScrollBars = ScrollBars.Vertical;
                                        edit.TextChanged += Edit_TextChanged;
                                        edit.Size = new Size(300, 20);
                                    }
                                    break;
                                #endregion
                                case "Int16":
                                case "Int32":
                                    #region Entier
                                    edit = new NumericUpDown();
                                    ((NumericUpDown)edit).Maximum = 10000;
                                    ((NumericUpDown)edit).Minimum = -13000;

                                    if (propriété.GetValue(objet) != null)
                                        ((NumericUpDown)edit).Value = (int)propriété.GetValue(objet);
                                    #endregion
                                    break;
                                case "Color":
                                    #region Couleur
                                    edit = new Label();
                                    ((Label)edit).BorderStyle = BorderStyle.Fixed3D;
                                    edit.Click += Couleur_Click;
                                    if (propriété.GetValue(objet) != null)
                                        edit.BackColor = (Color)propriété.GetValue(objet);
                                    break;
                                #endregion
                                case "Byte[]":
                                    #region Image
                                    edit = new ScrollPicture();
                                    ((ScrollPicture)edit).Changed += Image_Changed;
                                    ((ScrollPicture)edit).BorderStyle = BorderStyle.Fixed3D;
                                    ((ScrollPicture)edit).SetImage((Byte[])propriété.GetValue(objet));
                                    edit.Size = new Size(60, 80);
                                    #endregion;
                                    break;
                                default:
                                    #region Générique
                                    Type propertyType = propriété.PropertyType;
                                    if (propertyType.IsGenericType &&
                                        propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    { }
                                    Type t = Nullable.GetUnderlyingType(propriété.PropertyType);
                                    if (t != null)
                                    {
                                        if (propriété.Name.StartsWith("An"))
                                            switch (t.Name)
                                            {
                                                case "Int32":
                                                    edit = new NumericUpDown
                                                    {
                                                        Width = 50,
                                                        Maximum = 3000,
                                                        DecimalPlaces = 0
                                                    };
                                                    if (propriété.GetValue(objet) != null)
                                                        ((NumericUpDown)edit).Text = propriété.GetValue(objet).ToString();
                                                    break;
                                            }
                                        if (propriété.Name == "Durée")
                                        {
                                            switch (t.Name)
                                            {
                                                case "Int32":
                                                    edit = new NumericUpDown
                                                    {
                                                        Width = 50, 
                                                        Maximum = 3000, 
                                                        DecimalPlaces = 0
                                                    };
                                                    if (propriété.GetValue(objet) != null)
                                                        ((NumericUpDown)edit).Text = propriété.GetValue(objet).ToString();
                                                    break;
                                            }
                                        }
                                    }
                                    else
                                    {

                                    }
                                    break;
                                    #endregion;
                            }
                            #endregion
                            break;
                    }
                    if (edit != null)
                        edit.Tag = propriété;
                    #endregion
                }
                if (edit != null)
                {
                    #region Création des contrôles
                    edit.Location = new Point(lb.Location.X + lb.Width + 10, y);
                    if (propriété.PropertyType.Name == "Byte[]")
                    {
                        edit.Location = new Point(lb.Location.X + lb.Width + 10, y);
                        ScrollPicture sp = (ScrollPicture)edit;
                        if (sp.Image != null)
                        {
                            Size s = sp.Image.Size;

                            int scale = s.Height / 120;
                            if (scale == 0)
                                scale = 1;
                            sp.Size = new Size(s.Width / scale, s.Height / scale);
                        }
                        ((Control)edit).Location = new Point(lb.Location.X + lb.Width + 10, y);
                    }
                    if (edit is DataGridView dtg)
                    {
                        edit.Width = 400;
                    }
                    Controls.Add(lb);
                    Controls.Add(edit);
                    largeur = edit.Left + edit.Width + 10;
                    if (edit2 != null)
                    {
                        edit2.Tag = edit;
                        edit2.Width = 50;
                        edit2.Location = new Point(edit.Location.X + edit.Width + 10, y);
                        Controls.Add(edit2);
                        largeur = Math.Max(largeur, edit2.Left + edit2.Width + 10);
                    }
                    if (but != null)
                    {
                        but.Location = new Point(edit.Location.X + edit.Width + 10, y);
                        Controls.Add(but);
                        largeur = Math.Max(largeur, but.Left + but.Width + 10);
                    }
                    y += edit.Height + 10;
                    #endregion
                }
            }
            #endregion
            Height = y + 50;
            #region Boutons
            y = 30;
            largeur += 120;
            foreach (string s in new string[] { "OK", "Annuler" })
            {
                Button bouton = new Button
                {
                    Text = s
                };
                Controls.Add(bouton);
                bouton.Location = new Point(largeur, y);
                bouton.Click += Bouton_Click;
                y += bouton.Height + 5;
                Width = bouton.Left + bouton.Width + 40;
            }
            Height = Math.Max(Height, Bottom);
            #endregion
            Cursor = Cursors.Default;
        }

        private void Edit_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            if (edit.Text.Length > 100)
            {
                edit.Size = new Size(400, 100);
                ((TextBox)edit).Multiline = true;
                ((TextBox)edit).ScrollBars = ScrollBars.Both;
            }
            if ((edit.Text.Length > 20) & (edit.Text.Length < 100))
            {
                edit.Size = new Size(300, 20);
                ((TextBox)edit).ScrollBars = ScrollBars.Horizontal;
            }
            foreach(Control c in Controls)
            {
                if (c.Bottom > edit.Bottom)
                {
                    c.Top = edit.Bottom + 10;
                    Height = c.Top+c.Height + 80;
                }
            }
            Refresh();
        }
        #region Datagrid events
        private void Dtg_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataGridView dtg = (DataGridView)sender;
            //DataGridViewRow dr = e.Row;
            Roles r = (Roles)e.Row.Tag;
            Opéra opera = (Opéra)dtg.Tag;
            opera.Roles.Remove(r);
        }
        private void Dtg_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex == 0)
                return;
            DataGridView dtg = (DataGridView)sender;
            DataGridViewRow dr = dtg.Rows[e.RowIndex];
            if (String.IsNullOrEmpty((string)dr.Cells[0].EditedFormattedValue))
                return;
            if (dtg.Tag is Opéra opera)
            {
                Roles u = (Roles)dtg.Rows[e.RowIndex].Tag;
                if (u == null)
                {
                    u = new Roles();
                    dtg.Rows[e.RowIndex].Tag = u;
                }
                PropertyInfo[] pis = typeof(Roles).GetProperties();
                foreach (PropertyInfo pi in pis)
                {
                    if (pi.PropertyType.Name == "String")
                    {
                        pi.SetValue(u, (string)dr.Cells[pi.Name].EditedFormattedValue);
                    }
                }
                if (!opera.Roles.Contains(u))
                    opera.Roles.Add(u);
            }
        }
        private void Dtg_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0)
                return;
            DataGridView dtg = (DataGridView)sender;
            DataGridViewRow dr = dtg.Rows[e.RowIndex];
            if (String.IsNullOrEmpty((string)dr.Cells[0].EditedFormattedValue))
                return;
            if (dtg.Tag is Opéra opera)
            {
                Roles u = (Roles)dtg.Rows[e.RowIndex].Tag;
                if (u == null)
                    u = new Roles();
                PropertyInfo[] pis = typeof(Roles).GetProperties();
                foreach (PropertyInfo pi in pis)
                {
                    if (pi.PropertyType.Name == "String")
                    {
                        pi.SetValue(u, (string)dr.Cells[pi.Name].EditedFormattedValue);
                    }
                }
                if (!opera.Roles.Contains(u))
                    opera.Roles.Add(u);
            }
        }
        private void Dtg_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            //DataGridView dtg = (DataGridView)sender;
            //DataGridViewRow dr = e.Row;
            //Opéra op = (Opéra)dtg.Tag;
            Roles r = new Roles();
            e.Row.Tag = r;
        }
        #endregion
        private Control CréeBouton(PropertyInfo propriété)
        {
            Control but = new Button
            {
                Text = propriété.Name,
                Tag = propriété
            };
            but.Click += But_Click;
            return but;
        }
        private void Image_Changed(object sender, EventArgs e)
        {
            ScrollPicture sp = (ScrollPicture)sender;
            int i = Controls.IndexOf(sp);
            if (sp.Image != null)
            {
                Size s = sp.Image.Size;
                Size t = new Size
                {
                    Height = Math.Min(s.Height, Controls[i + 1].Top - sp.Top),

                };
                t.Width = s.Width * t.Height / s.Height;
                sp.Size = t;
            }
        }
        private void But_Click(object sender, EventArgs e)
        {
            PropertyInfo pi = (PropertyInfo)((Button)sender).Tag;
            Object objet = null;
            switch (pi.PropertyType.Name)
            {
                case "Editeur":
                    objet = new Editeur();
                    break;
                case "Musicien":
                    objet = new Musicien();
                    break;
                //case "Ville":
                //    objet = new Ville();
                //    break;
                case "Roles":
                    break;
                case "Opéra":
                    objet = null;
                    foreach (Control cl in Controls)
                    {
                        if (cl is ComboBox box)
                            if (cl.Name == pi.PropertyType.Name)
                            {
                                objet = (Opéra)box.SelectedItem;
                            }
                    }
                    if (objet == null) objet = new Opéra();// {Musicien = ((Disques)ObjetCourant).;
                    break;
                case "Orchestres":
                    objet = new Orchestres();
                    break;
                case "Salles":
                    objet = new Salles();
                    break;
                case "Instrument":
                    objet = new Instrument();
                    break;
                case "Pays":
                    objet = new Pays();
                    break;
                case "Airs":
                    foreach (Control cl in Controls)
                    {
                        if (cl is ComboBox box)
                            if (cl.Name == pi.PropertyType.Name)
                            {
                                objet = (Airs)box.SelectedItem;
                                break;
                            }
                    }
                    objet = new Airs { Opéra = ((Disques)currentNode.Tag).Opéra };
                    break;
            }
            MetaJour(pi, objet, currentNode);
        }
        private MiseAJourObjet MetaJour(PropertyInfo pi, Object objet, TreeNode tn = null)
        {
            MiseAJourObjet mf = new MiseAJourObjet(objet, tn);
            if (mf.ShowDialog() == DialogResult.OK)
            {
                foreach (Control s in Controls)
                {
                    if (s.Name == pi.Name)
                    {
                        if (s is ComboBox box)
                        {
                            box.Items.Add(objet);
                            box.SelectedItem = objet;
                        }
                    }
                }
            }
            return mf;
        }
        private static Control CréeCombo(object objet, PropertyInfo propriété, TreeNode tn = null)
        {
            Type t = objet.GetType();
            Control edit = new ComboBox
            {
                Width = 220,
                Name = propriété.Name
            };
            if (t.BaseType.Name == "Opéra")
            {
                edit.Name = t.BaseType.Name;
                MainForm.md.Opéra.Where(c => c.Musicien.Code_Musicien== ((Opéra)objet).Musicien.Code_Musicien).ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                ((ComboBox)edit).SelectedText = ((Opéra)objet).Titre;
                return edit;
            }            
            switch (propriété.Name)
            {
                case "Airs":
                    Disques disque = ((Marqueurs)objet).Disques;
                    MainForm.md.Airs.Where(c=>c.Opéra.Code_Opéra== disque.Opéra.Code_Opéra).OrderBy(c => c.Nom).ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    if (((Marqueurs)objet).Airs != null)
                        ((ComboBox)edit).SelectedItem = ((Marqueurs)objet).Airs;
                    break;
                case "Audio":
                    MainForm.md.Disques.Where(c => !String.IsNullOrEmpty(c.Audio)).Select(c => c.Audio).Distinct().ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    if (((Disques)objet).Audio != null)
                        ((ComboBox)edit).Text = ((Disques)objet).Audio; break;
                case "Ville":
                    MainForm.md.Salles.Where(c => !String.IsNullOrEmpty(c.Ville)).Select(c => c.Ville).Distinct().ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    if (((Salles)objet).Ville != null)
                        ((ComboBox)edit).Text = ((Salles)objet).Ville; ((ComboBox)edit).Sorted = true; break;
                case "Vidéo":
                    MainForm.md.Disques.Where(c => !String.IsNullOrEmpty(c.Vidéo)).Select(c => c.Vidéo).Distinct().ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    if (((Disques)objet).Vidéo != null)
                        ((ComboBox)edit).Text = ((Disques)objet).Vidéo;
                    break;
                case "Format":
                    MainForm.md.Disques.Where(c => !String.IsNullOrEmpty(c.Format)).Select(c => c.Format).Distinct().ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    if (((Disques)objet).Format != null)
                        ((ComboBox)edit).Text = ((Disques)objet).Format;
                    break;
                case "Définition":
                    MainForm.md.Définition.Distinct().ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    if (((Disques)objet).Définition != null)
                        ((ComboBox)edit).Text = ((Disques)objet).Définition.Détail;
                    break;
                case "Source":
                    MainForm.md.Disques.Where(c => !String.IsNullOrEmpty(c.Source)).Select(c => c.Source).Distinct().ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    break;
                case "Editeur":
                    MainForm.md.Editeur.Distinct().OrderBy(c => c.NomEditeur).ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    ((ComboBox)edit).SelectedItem = propriété.GetValue(objet);
                    break;
                case "Opéra":
                    string nom = "";
                    if (tn != null)
                    {
                        Musicien mus = null;
                        if (tn.Tag is Musicien musicien)
                        {
                            mus = musicien;
                        }
                        else if (tn.Parent.Tag is Musicien musicien1)
                            mus = musicien1;
                        if (mus != null)
                            nom = mus.Nom_Musicien;
                    }
                    MainForm.md.Opéra.Where(c => c.Musicien.Nom_Musicien.StartsWith(nom))
                        .OrderBy(c => c.Titre).ToList()
                        .ForEach(x => ((ComboBox)edit).Items.Add(x));
                    ((ComboBox)edit).SelectedItem = propriété.GetValue(objet);
                    break;
                case "Orchestres":
                    MainForm.md.Orchestres.Distinct().OrderBy(c => c.Nom_Orchestre).ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    ((ComboBox)edit).SelectedItem = propriété.GetValue(objet);
                    break;
                case "Disques":
                    MainForm.md.Disques.OrderBy(c=>c.Opéra.Titre).Distinct().ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    ((ComboBox)edit).SelectedItem = propriété.GetValue(objet);
                    break;
                case "Salles":
                    MainForm.md.Salles.Distinct().OrderBy(c=>c.Ville).ThenBy(c => c.Nom_Salle).ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    ((ComboBox)edit).SelectedItem = propriété.GetValue(objet);
                    break;
                case "Instrument":
                    MainForm.md.Instrument.Where(i => i.Type == "Voix" | i.Nom_Instrument == "Composition" | new[] { "Direction", "Metteur en Scène" }.Contains(i.Nom_Instrument)).Distinct().OrderBy(c => c.Nom_Instrument).ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    ((ComboBox)edit).SelectedItem = propriété.GetValue(objet);
                    break;
                case "Pays":
                    MainForm.md.Pays.Distinct().OrderBy(c => c.Nom_Pays).ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    ((ComboBox)edit).SelectedItem = propriété.GetValue(objet);
                    break;
            }
            return edit;
        }
        private void Couleur_Click(object sender, EventArgs e)
        {
            Label lb = (Label)sender;
            ColorDialog cld = new ColorDialog { Color = lb.BackColor };
            if (cld.ShowDialog() == DialogResult.OK)
                lb.BackColor = cld.Color;
        }
        private void Bouton_Click(object sender, EventArgs e)
        {
            switch (((Control)sender).Text)
            {
                case "Annuler":
                    break;
                case "OK":
                    DialogResult = DialogResult.OK;
                    foreach (Control c in Controls)
                    {
                        #region Mise à jour de l'objet courant
                        if (c.Tag != null && c.Tag.GetType().Name == "RuntimePropertyInfo")
                        {
                            PropertyInfo p = (PropertyInfo)c.Tag;
                            switch (p.PropertyType.Name)
                            {
                                case "Boolean":
                                    p.SetValue(ObjetCourant, ((CheckBox)c).Checked);
                                    break;
                                case "String":
                                    p.SetValue(ObjetCourant, c.Text);
                                    break;
                                case "Single":
                                case "Double":
                                    float value;
                                    if (float.TryParse(c.Text, out value))
                                    {
                                        if (value == 0)
                                            p.SetValue(ObjetCourant, null);
                                        else p.SetValue(ObjetCourant, value);
                                    }
                                    break;
                                case "Int32":
                                    int val;
                                    if (int.TryParse(c.Text, out val))
                                    {
                                        if (val == 0)
                                            p.SetValue(ObjetCourant, null);
                                        else p.SetValue(ObjetCourant, val);
                                    }
                                    break;
                                case "Color":
                                    p.SetValue(ObjetCourant, c.BackColor);
                                    break;
                                case "Font":
                                    p.SetValue(ObjetCourant, c.Font);
                                    break;
                                case "Point":
                                case "Size":
                                    #region Deux valeurs
                                    int x_coord; int y_coord;
                                    if (int.TryParse(c.Text, out x_coord))
                                    {
                                        foreach (Control cont in Controls)
                                        {
                                            if (cont.Tag == c)
                                            {
                                                if (int.TryParse(cont.Text, out y_coord))
                                                {
                                                    Object P = null;
                                                    if (p.PropertyType.Name == "Point")
                                                        P = new Point(x_coord, y_coord);
                                                    if (p.PropertyType.Name == "Size")
                                                        P = new Size(x_coord, y_coord);
                                                    p.SetValue(ObjetCourant, P);
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    #endregion
                                    break;
                                case "Type":
                                    switch (c.Text)
                                    {
                                        case "int":
                                            p.SetValue(ObjetCourant, typeof(int));
                                            break;
                                        case "float":
                                            p.SetValue(ObjetCourant, typeof(float));
                                            break;
                                        case "string":
                                            p.SetValue(ObjetCourant, typeof(string));
                                            break;
                                    }
                                    break;
                                case "Byte[]":
                                    p.SetValue(ObjetCourant, ((ScrollPicture)c).GetImage());
                                    break;
                                case "Nullable`1":
                                    if (Nullable.GetUnderlyingType(p.PropertyType) == typeof(Int32))
                                    {
                                        if (int.TryParse(c.Text, out int valuei))
                                        {
                                            if (valuei == 0)
                                                p.SetValue(ObjetCourant, null);
                                            else
                                                p.SetValue(ObjetCourant, valuei);
                                        }
                                    }
                                    break;
                                default:
                                    if (c.GetType().Name == "ComboBox")
                                    {
                                        if (currentNode.Tag is Marqueurs)
                                        {
                                            if (c.Name == "Opéra")
                                            {

                                            }
                                            else
                                                p.SetValue(ObjetCourant, ((ComboBox)c).SelectedItem);
                                        }
                                        else
                                            p.SetValue(ObjetCourant, ((ComboBox)c).SelectedItem);
                                    }
                                    break;
                            }
                        }
                        #endregion
                    }
                    #region Sauvegarde des nouveaux objets
                    switch (ObjetCourant)
                    {
                        case Musicien musicien:
                            if (musicien.Code_Musicien == 0)
                                MainForm.md.Musicien.Add(musicien);
                            break;
                        case Disques disque:
                            if (disque.Code_Disque == 0)
                                MainForm.md.Disques.Add(disque);
                            break;
                        case Opéra opera:
                            if (opera.Code_Opéra == 0)
                                MainForm.md.Opéra.Add(opera);
                            break;
                        case Interprétation interpretation:
                            if (interpretation.Code_Interprétation == 0)
                                MainForm.md.Interprétation.Add(interpretation);
                            break;
                        case Airs air:

                            if (air.Code_Air == 0)
                                MainForm.md.Airs.Add(air);
                            break;
                    }
                    //else if (ObjetCourant is Marqueurs marqueur)
                    //{
                    //    //if (air.Code_Air == 0)
                    //    //    md.Airs.Add(m);
                    //}
                    MainForm.md.SaveChanges();
                    #endregion
                    break;
            }
            Close();
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
