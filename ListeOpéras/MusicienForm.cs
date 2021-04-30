using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        Object ObjetCourant;
        Modèle modèle;
        private static List<Musicien> musiciens;
        public MiseAJourObjet(object o, Modèle md): this(o, md, null)
        {
            
        }
        public MiseAJourObjet(object objet, Modèle md, TreeNode tn)
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            //       musiciens = md.Musicien.OrderBy(c => c.Nom_Musicien).ThenBy(c => c.Prénom_Musicien).ToList();
            modèle = md;
            if (objet == null)
                return;
            Type tp = objet.GetType();
            if (tp.FullName.Contains("Proxies"))
                tp = tp.BaseType;
            Text = "Paramètres " + tp.Name;
            ObjetCourant = objet;
            PropertyInfo[] pis = tp.GetProperties();
            int y = 30;
            int largeur = 0;
            #region Propriétés 
            foreach (PropertyInfo propriété in pis)
            {
                Label lb = new Label();
                lb.Text = propriété.Name.Replace("_", " ");
                lb.Location = new Point(10, y);
                Control edit = null;
                Control edit2 = null;
                Control but = null;
                if(propriété.Name == "Musicien")
                {
                }
                if (propriété.PropertyType.Name.Contains("IColl"))
                {
                    #region Traitement des collections
                    if (objet is Opéra)
                    {
                        #region Cas d'un opéra
                        Opéra opera = (Opéra)objet;
                        if (propriété.Name == "Roles")
                        {
                            Controls.Add(lb);
                            DataGridView dtg = new DataGridView();
                            dtg.Name = "Roles";
                            dtg.AllowUserToAddRows = true;
                            dtg.AllowUserToDeleteRows = true;
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
                    #endregion
                }
                else  if ((propriété.SetMethod != null) & !propriété.Name.Contains("Code"))
                {
                    #region Traitement des propriétes simples
                    switch (propriété.Name)
                    {
                        #region En fonction du nom de propriété
                        case "Musicien":
                            #region Musicien
                            edit = new ComboBox();
                            edit.Name = propriété.Name;
                            edit.Width = 180;
                            if (ObjetCourant.GetType().Name.Contains("Opéra"))
                            {
                                musiciens = md.Musicien.Where(c => c.Instrument.Nom_Instrument == "Composition").OrderBy(c => c.Nom_Musicien).ToList();
                            }
                            if (ObjetCourant.GetType().Name.Contains("Disques") | ObjetCourant.GetType().Name.Contains("Interprétation") | ObjetCourant.GetType().Name.Contains("Diriger") | ObjetCourant.GetType().Name.Contains("MiseEnScene"))
                            {
                                musiciens = md.Musicien.Where(c => c.Instrument.Type == "Voix" | c.Instrument.Nom_Instrument == "Direction" | c.Instrument.Nom_Instrument == "Metteur En Scène").OrderBy(c => c.Nom_Musicien).ToList();
                            }
                            musiciens.ForEach(x => ((ComboBox)edit).Items.Add(x));
                            ((ComboBox)edit).SelectedItem = (Musicien)propriété.GetValue(objet);
                            #endregion
                            but = CréeBouton(propriété);
                            break;
                        case "Opéra":
                            edit = CréeCombo(objet, md, propriété, tn);
                            but = CréeBouton(propriété);
                            break;
                        case "Editeur":
                        case "Instrument":
                        case "Orchestres":
                        case "Pays":
                        case "Salles":
                            edit= CréeCombo(objet, md, propriété);
                            but = CréeBouton(propriété);
                            break;
                        case "Ville":
                        case "Vidéo":
                        case "Audio":
                        case "Définition":
                        case "Détail_Définition":
                        case "Format":
                        case "Source":
                            edit = CréeCombo(objet, md, propriété);
                            break;
                        case "Roles":
                            //#region Role
                            //edit = new ComboBox();
                            //edit.Width = 180;
                            //edit.Name = propriété.Name;
                            //int op = ((Interprétation)objet).Disques.Opéra.Code_Opéra;
                            //foreach (Roles pay in md.Roles.Where(x => x.Opéra.Code_Opéra == op).OrderBy(c => c.Role))
                            //{
                            //    ((ComboBox)edit).Items.Add(pay);
                            //}
                            //Roles curRole = (Roles)propriété.GetValue(objet);
                            //((ComboBox)edit).SelectedItem = curRole;

                            //but = new Button();
                            //but.Text = propriété.Name;
                            //but.Tag = propriété;
                            //but.Click += But_Click;

                            //#endregion
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
                                    edit = new NumericUpDown();
                                    edit.Width = 50;
                                    ((NumericUpDown)edit).Maximum = 1000;
                                    ((NumericUpDown)edit).DecimalPlaces = 2;
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
                                    edit = new TextBox();
                                    edit.Name = propriété.Name;
                                    if (propriété.GetValue(objet) != null)
                                    {
                                        edit.Text = (String)propriété.GetValue(objet);
                                        if (edit.Text.Length > 100)
                                        {
                                            edit.Size = new Size(400, 100);
                                            ((TextBox)edit).Multiline = true;
                                            ((TextBox)edit).ScrollBars = ScrollBars.Both;
                                        }
                                        if((edit.Text.Length>20)& (edit.Text.Length <100))
                                        {
                                            edit.Size = new Size(400, 20);
                                            ((TextBox)edit).ScrollBars = ScrollBars.Horizontal;
                                        }
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
                                                    edit = new NumericUpDown();
                                                    edit.Width = 50;
                                                    ((NumericUpDown)edit).Maximum = 3000;
                                                    ((NumericUpDown)edit).DecimalPlaces = 0;
                                                    if (propriété.GetValue(objet) != null)
                                                        ((NumericUpDown)edit).Text = propriété.GetValue(objet).ToString();
                                                    break;
                                            }
                                        if (propriété.Name == "Durée")
                                        {
                                            switch (t.Name)
                                            {
                                                case "Int32":
                                                    edit = new NumericUpDown();
                                                    edit.Width = 50;
                                                    ((NumericUpDown)edit).Maximum = 3000;
                                                    ((NumericUpDown)edit).DecimalPlaces = 0;
                                                    if (propriété.GetValue(objet) != null)
                                                        ((NumericUpDown)edit).Text = propriété.GetValue(objet).ToString();
                                                    break;
                                            }
                                        }
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
                        edit.Size = new Size(180, 120);
                        ScrollPicture sp = (ScrollPicture)edit;
                        if (sp.Image != null)
                        {
                            Size s = sp.Image.Size;
                            int scale = 2;
                            sp.Size = new Size(s.Width / scale, s.Height / scale);
                        }
                        ((Control)edit).Location = new Point(lb.Location.X + lb.Width + 10, y);
                    }
                    if(edit is DataGridView dtg)
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
            largeur += 70;
            foreach (string s in new string[] { "OK", "Annuler" })
            {
                Button bouton = new Button();
                bouton.Text = s;
                Controls.Add(bouton);
                bouton.Location = new Point(largeur, y);
                bouton.Click += Bouton_Click;
                y += bouton.Height + 5;
                Width = bouton.Left + bouton.Width + 40;
            }
            Height = Math.Max(Height, Bottom);
            #endregion
        }
        #region Datagrid events
        private void Dtg_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataGridView dtg = (DataGridView)sender;
            DataGridViewRow dr = e.Row;
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
            Opéra opera = (Opéra)dtg.Tag;
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
    //        e.Cancel = true;
        }
        private void Dtg_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0)
                return;
            DataGridView dtg = (DataGridView)sender;
            DataGridViewRow dr = dtg.Rows[e.RowIndex];
            if (String.IsNullOrEmpty((string)dr.Cells[0].EditedFormattedValue))
                return;
            Opéra opera = (Opéra)dtg.Tag;
            Roles u =(Roles) dtg.Rows[e.RowIndex].Tag;
            if (u == null)
                u = new Roles();
            PropertyInfo[] pis = typeof(Roles).GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if(pi.PropertyType.Name=="String")
                {
                    pi.SetValue(u,(string) dr.Cells[pi.Name].EditedFormattedValue);
                }
            }
            if (!opera.Roles.Contains(u))
                opera.Roles.Add(u);
        }
        private void Dtg_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataGridView dtg = (DataGridView)sender;
            DataGridViewRow dr = e.Row;
            Opéra op = (Opéra)dtg.Tag;
            Roles r = new Roles();
            e.Row.Tag = r;
        }
        #endregion
        private Control CréeBouton(PropertyInfo propriété)
        {
            Control but = new Button();
            but.Text = propriété.Name;
            but.Tag = propriété;
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
                Size t = new Size();
                t.Height = Math.Min(s.Height, Controls[i + 1].Top - sp.Top);
                t.Width = s.Width * t.Height / s.Height;
                sp.Size = t;
            }
        }
        private void But_Click(object sender, EventArgs e)
        {
            PropertyInfo pi = (PropertyInfo)((Button)sender).Tag;
            MiseAJourObjet mf;
            Object objet = null;
            switch (pi.PropertyType.Name)
            {
                case "Musicien":
                   objet = new Musicien();
                    break;
                //case "Ville":
                //    objet = new Vi();
                //    break;
                case "Roles":
                    break;
                case "Opéra":
                    objet = null;
                    foreach (Control cl in Controls)
                    {
                        if (cl is ComboBox)
                            if (cl.Name == pi.PropertyType.Name)
                            {
                                objet = (Opéra)((ComboBox)cl).SelectedItem;
                            }
                    }
                    if (objet == null) objet= new Opéra();// {Musicien = ((Disques)ObjetCourant).;
                    break;
                case "Orchestres":
                   objet = new Orchestres();
                    break;
                case "Salles":
                    objet = new Salles();
                    break;
                case "Instrument":
                   objet= new Instrument();
                    break;
                case "Pays":
                   objet= new Pays();
                    break;
            }
                    mf = MetaJour(pi, objet);
        }
        private MiseAJourObjet MetaJour(PropertyInfo pi, Object objet)
        {
            MiseAJourObjet mf = new MiseAJourObjet(objet, modèle);
            if (mf.ShowDialog() == DialogResult.OK)
            {
                foreach (Control s in Controls)
                {
                    if (s.Name == pi.Name)
                    {
                        if (s is ComboBox)
                        {
                            ((ComboBox)s).Items.Add(objet);
                            ((ComboBox)s).SelectedItem = objet;
                        }
                    }
                }
            }
            return mf;
        }
        private static Control CréeCombo(object objet, Modèle md, PropertyInfo propriété, TreeNode tn = null)
        {
            Type t = objet.GetType();
            Control edit = new ComboBox();
            edit.Width = 220;
            edit.Name = propriété.Name;
            switch (propriété.Name)
            {
                case "Audio":
                    md.Disques.Where(c => !String.IsNullOrEmpty(c.Audio)).Select(c => c.Audio).Distinct().ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    if (((Disques)objet).Audio != null)
                        ((ComboBox)edit).Text = ((Disques)objet).Audio; break;
                case "Ville":
                    md.Salles.Where(c => !String.IsNullOrEmpty(c.Ville)).Select(c => c.Ville).Distinct().ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    if (((Salles)objet).Ville != null)
                        ((ComboBox)edit).Text = ((Salles)objet).Ville; ((ComboBox)edit).Sorted = true; break;
                case "Vidéo":
                    md.Disques.Where(c => !String.IsNullOrEmpty(c.Vidéo)).Select(c => c.Vidéo).Distinct().ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    if (((Disques)objet).Vidéo != null)
                        ((ComboBox)edit).Text = ((Disques)objet).Vidéo;
                    break;
                case "Format":
                    md.Disques.Where(c => !String.IsNullOrEmpty(c.Format)).Select(c => c.Format).Distinct().ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    if (((Disques)objet).Format != null)
                        ((ComboBox)edit).Text = ((Disques)objet).Format;
                    break;
                case "Définition":
                    md.Disques.Where(c => !String.IsNullOrEmpty(c.Définition)).Select(c => c.Définition).Distinct().ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    if (((Disques)objet).Définition != null)
                        ((ComboBox)edit).Text = ((Disques)objet).Définition;
                    break;
                case "Source":
                    md.Disques.Where(c => !String.IsNullOrEmpty(c.Source)).Select(c => c.Source).Distinct().ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    if (((Disques)objet).Définition != null)
                        ((ComboBox)edit).Text = ((Disques)objet).Source;
                    break;
                case "Détail_Définition":
                    md.Définition.Distinct().OrderBy(c => c.Détail).ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    if (((Disques)objet).Définition1 != null)
                        ((ComboBox)edit).Text = ((Disques)objet).Définition1.Détail;
                    break;
                case "Editeur":
                    md.Editeur.Distinct().OrderBy(c => c.Editeur1).ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    ((ComboBox)edit).SelectedItem = propriété.GetValue(objet);
                    break;
                case "Opéra":
                    string nom = "";
                    if (tn != null)
                    {
                //        Musicien mus = (Musicien)tn.Tag;
                 //       nom = mus.Nom_Musicien;
                    }
                    md.Opéra.Where(c => c.Musicien.Nom_Musicien.StartsWith(nom))
                        .OrderBy(c => c.Titre).ToList()
                        .ForEach(x => ((ComboBox)edit).Items.Add(x));
                    ((ComboBox)edit).SelectedItem = propriété.GetValue(objet);
                    break;
                case "Orchestres":
                    md.Orchestres.Distinct().OrderBy(c => c.Nom_Orchestre).ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    ((ComboBox)edit).SelectedItem = propriété.GetValue(objet);
                    break;
                case "Salles":
                    md.Salles.Distinct().OrderBy(c => c.Nom_Salle).ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    ((ComboBox)edit).SelectedItem = propriété.GetValue(objet);
                    break;
                case "Instrument":
                    md.Instrument.Where(i=>i.Type=="Voix"|new[] { "Direction", "Metteur en Scène" }.Contains(i.Nom_Instrument)).Distinct().OrderBy(c => c.Nom_Instrument).ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
                    ((ComboBox)edit).SelectedItem = propriété.GetValue(objet);
                    break;
                case "Pays":
                    md.Pays.Distinct().OrderBy(c => c.Nom_Pays).ToList().ForEach(x => ((ComboBox)edit).Items.Add(x));
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
                                        int valuei;
                                        if (int.TryParse(c.Text, out valuei))
                                        {
                                            if (valuei == 0)
                                                p.SetValue(ObjetCourant, null);
                                            else
                                                p.SetValue(ObjetCourant, valuei);
                                        }
                                    }
                                    break;
                                default:
                                    if (c.GetType().Name=="ComboBox")
                                    {
                                        p.SetValue(ObjetCourant, ((ComboBox)c).SelectedItem);
                                    }
                                    break;
                            }
                        }
                        #endregion
                    }
                    #region Sauvegarde des nouveaux objets
                    if (ObjetCourant is Musicien musi)
                    {
                        if (musi.Code_Musicien == 0)
                        {
                            modèle.Musicien.Add(musi);
                        }
                    }
                    if (ObjetCourant is Disques disq)
                    {
                        if (disq.Code_Disque == 0)
                        {
                            modèle.Disques.Add(disq);
                        }
                    }
                    if (ObjetCourant is Opéra oper)
                    {
                        if (oper.Code_Opéra == 0)
                        {
                            modèle.Opéra.Add(oper);
                        }
                    }
                    modèle.SaveChanges();
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
            if (keyData == Keys.Delete)
            {
                foreach (Control ct in Controls)
                {
                    if (ct is DataGridView)
                    {
                        DataGridView dtg = (DataGridView)ct;
                    }
                }
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
