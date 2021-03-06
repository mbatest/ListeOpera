namespace ListeOpéras
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                md.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.refresh = new System.Windows.Forms.ToolStripButton();
            this.hide = new System.Windows.Forms.ToolStripButton();
            this.traductionSousTitre = new System.Windows.Forms.ToolStripButton();
            this.Nouveau = new System.Windows.Forms.ToolStripButton();
            this.titre = new System.Windows.Forms.ToolStripComboBox();
            this.chanteurs = new System.Windows.Forms.ToolStripComboBox();
            this.nombreOpéras = new System.Windows.Forms.ToolStripLabel();
            this.achetés = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Longueur = new System.Windows.Forms.ToolStripLabel();
            this.splitFichiers = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabFichiers = new System.Windows.Forms.TabPage();
            this.arbreFichiers = new System.Windows.Forms.TreeView();
            this.tabOpéras = new System.Windows.Forms.TabPage();
            this.arbreOpéras = new System.Windows.Forms.TreeView();
            this.tabMusiciens = new System.Windows.Forms.TabPage();
            this.arbreMusiciens = new System.Windows.Forms.TreeView();
            this.tabRoles = new System.Windows.Forms.TabPage();
            this.arbreRoles = new System.Windows.Forms.TreeView();
            this.tabDirection = new System.Windows.Forms.TabPage();
            this.arbreDirection = new System.Windows.Forms.TreeView();
            this.tabMise = new System.Windows.Forms.TabPage();
            this.arbreMise = new System.Windows.Forms.TreeView();
            this.sound = new Controles.CustomButton();
            this.volume = new System.Windows.Forms.Label();
            this.zoomInButton = new Controles.CustomButton();
            this.zoomOutButton = new Controles.CustomButton();
            this.Audio = new System.Windows.Forms.Button();
            this.next = new Controles.CustomButton();
            this.prev = new Controles.CustomButton();
            this.pause = new Controles.CustomButton();
            this.position2 = new System.Windows.Forms.Label();
            this.positionSlider = new Controles.CustomSlider();
            this.position1 = new System.Windows.Forms.MaskedTextBox();
            this.balanceDial = new Controles.Dial();
            this.volumeDial = new Controles.Dial();
            this.panel = new System.Windows.Forms.Panel();
            this.Argument = new System.Windows.Forms.RichTextBox();
            this.sousTitre = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitFichiers)).BeginInit();
            this.splitFichiers.Panel1.SuspendLayout();
            this.splitFichiers.Panel2.SuspendLayout();
            this.splitFichiers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabFichiers.SuspendLayout();
            this.tabOpéras.SuspendLayout();
            this.tabMusiciens.SuspendLayout();
            this.tabRoles.SuspendLayout();
            this.tabDirection.SuspendLayout();
            this.tabMise.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionSlider)).BeginInit();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitFichiers);
            this.splitContainer2.Size = new System.Drawing.Size(1040, 563);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refresh,
            this.hide,
            this.traductionSousTitre,
            this.Nouveau,
            this.titre,
            this.chanteurs,
            this.nombreOpéras,
            this.achetés,
            this.toolStripSeparator1,
            this.Longueur});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1040, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // refresh
            // 
            this.refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refresh.Image = global::ListeOpéras.Properties.Resources._112_RefreshArrow_Green_32x32_72;
            this.refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(23, 22);
            this.refresh.Text = "Rafraichir";
            this.refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // hide
            // 
            this.hide.CheckOnClick = true;
            this.hide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.hide.Image = global::ListeOpéras.Properties.Resources.HideMember_6755;
            this.hide.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.hide.Name = "hide";
            this.hide.Size = new System.Drawing.Size(23, 22);
            this.hide.Text = "toolStripButton1";
            this.hide.Click += new System.EventHandler(this.Hide_Click);
            // 
            // traductionSousTitre
            // 
            this.traductionSousTitre.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.traductionSousTitre.Image = global::ListeOpéras.Properties.Resources.HiddenField_6028;
            this.traductionSousTitre.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.traductionSousTitre.Name = "traductionSousTitre";
            this.traductionSousTitre.Size = new System.Drawing.Size(23, 22);
            this.traductionSousTitre.Text = "sous titres";
            this.traductionSousTitre.Click += new System.EventHandler(this.TraductionSousTitre_Click);
            // 
            // Nouveau
            // 
            this.Nouveau.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Nouveau.Image = global::ListeOpéras.Properties.Resources.NewDocument_32x32;
            this.Nouveau.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Nouveau.Name = "Nouveau";
            this.Nouveau.Size = new System.Drawing.Size(23, 22);
            this.Nouveau.Text = "Saisie d\'un nouveau disque";
            this.Nouveau.Click += new System.EventHandler(this.Nouveau_Click);
            // 
            // titre
            // 
            this.titre.Name = "titre";
            this.titre.Size = new System.Drawing.Size(301, 25);
            this.titre.ToolTipText = "Choix de l\'opéra";
            this.titre.SelectedIndexChanged += new System.EventHandler(this.Titre_SelectedIndexChanged);
            // 
            // chanteurs
            // 
            this.chanteurs.Name = "chanteurs";
            this.chanteurs.Size = new System.Drawing.Size(151, 25);
            this.chanteurs.ToolTipText = "Choix musicine";
            this.chanteurs.SelectedIndexChanged += new System.EventHandler(this.Chanteurs_SelectedIndexChanged);
            // 
            // nombreOpéras
            // 
            this.nombreOpéras.Name = "nombreOpéras";
            this.nombreOpéras.Size = new System.Drawing.Size(19, 22);
            this.nombreOpéras.Text = "    ";
            // 
            // achetés
            // 
            this.achetés.Name = "achetés";
            this.achetés.Size = new System.Drawing.Size(13, 22);
            this.achetés.Text = "  ";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // Longueur
            // 
            this.Longueur.Name = "Longueur";
            this.Longueur.Size = new System.Drawing.Size(67, 22);
            this.Longueur.Text = "                    ";
            // 
            // splitFichiers
            // 
            this.splitFichiers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitFichiers.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitFichiers.IsSplitterFixed = true;
            this.splitFichiers.Location = new System.Drawing.Point(0, 0);
            this.splitFichiers.Name = "splitFichiers";
            // 
            // splitFichiers.Panel1
            // 
            this.splitFichiers.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitFichiers.Panel2
            // 
            this.splitFichiers.Panel2.Controls.Add(this.panel);
            this.splitFichiers.Size = new System.Drawing.Size(1040, 534);
            this.splitFichiers.SplitterDistance = 345;
            this.splitFichiers.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.splitContainer1.Panel2.Controls.Add(this.sound);
            this.splitContainer1.Panel2.Controls.Add(this.volume);
            this.splitContainer1.Panel2.Controls.Add(this.zoomInButton);
            this.splitContainer1.Panel2.Controls.Add(this.zoomOutButton);
            this.splitContainer1.Panel2.Controls.Add(this.Audio);
            this.splitContainer1.Panel2.Controls.Add(this.next);
            this.splitContainer1.Panel2.Controls.Add(this.prev);
            this.splitContainer1.Panel2.Controls.Add(this.pause);
            this.splitContainer1.Panel2.Controls.Add(this.position2);
            this.splitContainer1.Panel2.Controls.Add(this.positionSlider);
            this.splitContainer1.Panel2.Controls.Add(this.position1);
            this.splitContainer1.Panel2.Controls.Add(this.balanceDial);
            this.splitContainer1.Panel2.Controls.Add(this.volumeDial);
            this.splitContainer1.Size = new System.Drawing.Size(345, 534);
            this.splitContainer1.SplitterDistance = 448;
            this.splitContainer1.TabIndex = 1;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabFichiers);
            this.tabControl.Controls.Add(this.tabOpéras);
            this.tabControl.Controls.Add(this.tabMusiciens);
            this.tabControl.Controls.Add(this.tabRoles);
            this.tabControl.Controls.Add(this.tabDirection);
            this.tabControl.Controls.Add(this.tabMise);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(345, 448);
            this.tabControl.TabIndex = 2;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // tabFichiers
            // 
            this.tabFichiers.Controls.Add(this.arbreFichiers);
            this.tabFichiers.Location = new System.Drawing.Point(4, 22);
            this.tabFichiers.Name = "tabFichiers";
            this.tabFichiers.Padding = new System.Windows.Forms.Padding(3);
            this.tabFichiers.Size = new System.Drawing.Size(337, 422);
            this.tabFichiers.TabIndex = 4;
            this.tabFichiers.Text = "Fichiers";
            this.tabFichiers.UseVisualStyleBackColor = true;
            // 
            // arbreFichiers
            // 
            this.arbreFichiers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arbreFichiers.FullRowSelect = true;
            this.arbreFichiers.Location = new System.Drawing.Point(3, 3);
            this.arbreFichiers.Name = "arbreFichiers";
            this.arbreFichiers.Size = new System.Drawing.Size(331, 416);
            this.arbreFichiers.TabIndex = 0;
            this.arbreFichiers.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ArbreFichiers_AfterSelect);
            this.arbreFichiers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ArbreFichiers_MouseDoubleClick);
            // 
            // tabOpéras
            // 
            this.tabOpéras.Controls.Add(this.arbreOpéras);
            this.tabOpéras.Location = new System.Drawing.Point(4, 22);
            this.tabOpéras.Name = "tabOpéras";
            this.tabOpéras.Size = new System.Drawing.Size(337, 422);
            this.tabOpéras.TabIndex = 2;
            this.tabOpéras.Text = "Opéras";
            this.tabOpéras.UseVisualStyleBackColor = true;
            // 
            // arbreOpéras
            // 
            this.arbreOpéras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arbreOpéras.Location = new System.Drawing.Point(0, 0);
            this.arbreOpéras.Name = "arbreOpéras";
            this.arbreOpéras.Size = new System.Drawing.Size(337, 422);
            this.arbreOpéras.TabIndex = 0;
            this.arbreOpéras.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ArbreOpéras_AfterSelect);
            // 
            // tabMusiciens
            // 
            this.tabMusiciens.Controls.Add(this.arbreMusiciens);
            this.tabMusiciens.Location = new System.Drawing.Point(4, 22);
            this.tabMusiciens.Name = "tabMusiciens";
            this.tabMusiciens.Size = new System.Drawing.Size(337, 422);
            this.tabMusiciens.TabIndex = 3;
            this.tabMusiciens.Text = "Musiciens";
            this.tabMusiciens.UseVisualStyleBackColor = true;
            // 
            // arbreMusiciens
            // 
            this.arbreMusiciens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arbreMusiciens.Location = new System.Drawing.Point(0, 0);
            this.arbreMusiciens.Name = "arbreMusiciens";
            this.arbreMusiciens.Size = new System.Drawing.Size(337, 422);
            this.arbreMusiciens.TabIndex = 0;
            // 
            // tabRoles
            // 
            this.tabRoles.Controls.Add(this.arbreRoles);
            this.tabRoles.Location = new System.Drawing.Point(4, 22);
            this.tabRoles.Name = "tabRoles";
            this.tabRoles.Size = new System.Drawing.Size(337, 422);
            this.tabRoles.TabIndex = 7;
            this.tabRoles.Text = "Roles";
            this.tabRoles.UseVisualStyleBackColor = true;
            // 
            // arbreRoles
            // 
            this.arbreRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arbreRoles.Location = new System.Drawing.Point(0, 0);
            this.arbreRoles.Name = "arbreRoles";
            this.arbreRoles.Size = new System.Drawing.Size(337, 422);
            this.arbreRoles.TabIndex = 0;
            // 
            // tabDirection
            // 
            this.tabDirection.Controls.Add(this.arbreDirection);
            this.tabDirection.Location = new System.Drawing.Point(4, 22);
            this.tabDirection.Name = "tabDirection";
            this.tabDirection.Size = new System.Drawing.Size(337, 422);
            this.tabDirection.TabIndex = 5;
            this.tabDirection.Text = "Direction";
            this.tabDirection.UseVisualStyleBackColor = true;
            // 
            // arbreDirection
            // 
            this.arbreDirection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arbreDirection.Location = new System.Drawing.Point(0, 0);
            this.arbreDirection.Name = "arbreDirection";
            this.arbreDirection.Size = new System.Drawing.Size(337, 422);
            this.arbreDirection.TabIndex = 1;
            // 
            // tabMise
            // 
            this.tabMise.Controls.Add(this.arbreMise);
            this.tabMise.Location = new System.Drawing.Point(4, 22);
            this.tabMise.Name = "tabMise";
            this.tabMise.Size = new System.Drawing.Size(337, 422);
            this.tabMise.TabIndex = 6;
            this.tabMise.Text = "Mise en Scène";
            this.tabMise.UseVisualStyleBackColor = true;
            // 
            // arbreMise
            // 
            this.arbreMise.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arbreMise.Location = new System.Drawing.Point(0, 0);
            this.arbreMise.Name = "arbreMise";
            this.arbreMise.Size = new System.Drawing.Size(337, 422);
            this.arbreMise.TabIndex = 0;
            // 
            // sound
            // 
            this.sound.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.sound.Font = new System.Drawing.Font("Webdings", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.sound.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(176)))), ((int)(((byte)(143)))));
            this.sound.Image = global::ListeOpéras.Properties.Resources.speaker1;
            this.sound.Location = new System.Drawing.Point(77, 1);
            this.sound.Name = "sound";
            this.sound.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.sound.Size = new System.Drawing.Size(28, 22);
            this.sound.TabIndex = 23;
            this.sound.Text = "X";
            this.sound.UseVisualStyleBackColor = true;
            this.sound.Click += new System.EventHandler(this.Sound_Click);
            // 
            // volume
            // 
            this.volume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.volume.AutoSize = true;
            this.volume.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.volume.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(173)))), ((int)(((byte)(146)))));
            this.volume.Location = new System.Drawing.Point(88, 26);
            this.volume.Name = "volume";
            this.volume.Size = new System.Drawing.Size(55, 24);
            this.volume.TabIndex = 22;
            this.volume.Text = "0,000";
            // 
            // zoomInButton
            // 
            this.zoomInButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.zoomInButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.zoomInButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.zoomInButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zoomInButton.Font = new System.Drawing.Font("Webdings", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.zoomInButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(176)))), ((int)(((byte)(143)))));
            this.zoomInButton.Location = new System.Drawing.Point(290, 0);
            this.zoomInButton.Name = "zoomInButton";
            this.zoomInButton.Size = new System.Drawing.Size(28, 24);
            this.zoomInButton.TabIndex = 21;
            this.zoomInButton.Text = "";
            this.zoomInButton.UseVisualStyleBackColor = true;
            this.zoomInButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ZoomInButton_MouseDown);
            // 
            // zoomOutButton
            // 
            this.zoomOutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.zoomOutButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.zoomOutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zoomOutButton.Font = new System.Drawing.Font("Webdings", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.zoomOutButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(176)))), ((int)(((byte)(143)))));
            this.zoomOutButton.Location = new System.Drawing.Point(316, 0);
            this.zoomOutButton.Name = "zoomOutButton";
            this.zoomOutButton.Size = new System.Drawing.Size(28, 24);
            this.zoomOutButton.TabIndex = 20;
            this.zoomOutButton.Text = "";
            this.zoomOutButton.UseVisualStyleBackColor = true;
            this.zoomOutButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ZoomOutButton_MouseDown);
            // 
            // Audio
            // 
            this.Audio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Audio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.Audio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Audio.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Audio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(173)))), ((int)(((byte)(143)))));
            this.Audio.Location = new System.Drawing.Point(270, 0);
            this.Audio.Name = "Audio";
            this.Audio.Size = new System.Drawing.Size(75, 24);
            this.Audio.TabIndex = 19;
            this.Audio.Text = "Audio";
            this.Audio.UseVisualStyleBackColor = false;
            this.Audio.Visible = false;
            this.Audio.Click += new System.EventHandler(this.Audio_Click);
            // 
            // next
            // 
            this.next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.next.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.next.Font = new System.Drawing.Font("Webdings", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.next.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(176)))), ((int)(((byte)(143)))));
            this.next.Location = new System.Drawing.Point(51, 1);
            this.next.Name = "next";
            this.next.Size = new System.Drawing.Size(28, 22);
            this.next.TabIndex = 18;
            this.next.Text = "";
            this.next.UseVisualStyleBackColor = true;
            this.next.Click += new System.EventHandler(this.Next_Click);
            // 
            // prev
            // 
            this.prev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.prev.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.prev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.prev.Font = new System.Drawing.Font("Webdings", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.prev.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(176)))), ((int)(((byte)(143)))));
            this.prev.Location = new System.Drawing.Point(25, 1);
            this.prev.Name = "prev";
            this.prev.Size = new System.Drawing.Size(28, 22);
            this.prev.TabIndex = 17;
            this.prev.Text = "";
            this.prev.UseVisualStyleBackColor = true;
            this.prev.Click += new System.EventHandler(this.Prev_Click);
            // 
            // pause
            // 
            this.pause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pause.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pause.Font = new System.Drawing.Font("Webdings", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.pause.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(176)))), ((int)(((byte)(143)))));
            this.pause.Location = new System.Drawing.Point(0, 1);
            this.pause.Name = "pause";
            this.pause.Size = new System.Drawing.Size(28, 22);
            this.pause.TabIndex = 16;
            this.pause.Text = ";";
            this.pause.UseVisualStyleBackColor = true;
            this.pause.Click += new System.EventHandler(this.Pause_Click);
            // 
            // position2
            // 
            this.position2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.position2.AutoSize = true;
            this.position2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.position2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(173)))), ((int)(((byte)(146)))));
            this.position2.Location = new System.Drawing.Point(262, 27);
            this.position2.Name = "position2";
            this.position2.Size = new System.Drawing.Size(80, 24);
            this.position2.TabIndex = 11;
            this.position2.Text = "00:00:00";
            // 
            // positionSlider
            // 
            this.positionSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.positionSlider.AutoSize = false;
            this.positionSlider.Location = new System.Drawing.Point(6, 56);
            this.positionSlider.Name = "positionSlider";
            this.positionSlider.Size = new System.Drawing.Size(336, 25);
            this.positionSlider.TabIndex = 15;
            // 
            // position1
            // 
            this.position1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.position1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.position1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.position1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.position1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(173)))), ((int)(((byte)(146)))));
            this.position1.Location = new System.Drawing.Point(2, 27);
            this.position1.Mask = "00:00:00";
            this.position1.Name = "position1";
            this.position1.Size = new System.Drawing.Size(80, 22);
            this.position1.TabIndex = 10;
            this.position1.Text = "000000";
            this.position1.Enter += new System.EventHandler(this.Position1_Enter);
            this.position1.Leave += new System.EventHandler(this.Position1_Leave);
            this.position1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Position1_MouseDown);
            // 
            // balanceDial
            // 
            this.balanceDial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.balanceDial.Image = global::ListeOpéras.Properties.Resources.Dial_Normal_2;
            this.balanceDial.Location = new System.Drawing.Point(196, 5);
            this.balanceDial.MaximumSize = new System.Drawing.Size(55, 55);
            this.balanceDial.MinimumSize = new System.Drawing.Size(55, 55);
            this.balanceDial.Name = "balanceDial";
            this.balanceDial.Size = new System.Drawing.Size(55, 55);
            this.balanceDial.TabIndex = 14;
            this.balanceDial.Text = "dial1";
            this.balanceDial.Value = 0;
            // 
            // volumeDial
            // 
            this.volumeDial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.volumeDial.Image = global::ListeOpéras.Properties.Resources.Dial_Normal_2;
            this.volumeDial.Location = new System.Drawing.Point(143, 5);
            this.volumeDial.MaximumSize = new System.Drawing.Size(55, 55);
            this.volumeDial.MinimumSize = new System.Drawing.Size(55, 55);
            this.volumeDial.Name = "volumeDial";
            this.volumeDial.Size = new System.Drawing.Size(55, 55);
            this.volumeDial.TabIndex = 13;
            this.volumeDial.Text = "dial1";
            this.volumeDial.Value = 0;
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.panel.Controls.Add(this.Argument);
            this.panel.Controls.Add(this.sousTitre);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(691, 534);
            this.panel.TabIndex = 9;
            // 
            // Argument
            // 
            this.Argument.Location = new System.Drawing.Point(99, 93);
            this.Argument.Name = "Argument";
            this.Argument.Size = new System.Drawing.Size(416, 348);
            this.Argument.TabIndex = 8;
            this.Argument.Text = "";
            this.Argument.Visible = false;
            // 
            // sousTitre
            // 
            this.sousTitre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sousTitre.AutoSize = true;
            this.sousTitre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.sousTitre.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.sousTitre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(173)))), ((int)(((byte)(143)))));
            this.sousTitre.Location = new System.Drawing.Point(167, 482);
            this.sousTitre.Name = "sousTitre";
            this.sousTitre.Size = new System.Drawing.Size(316, 24);
            this.sousTitre.TabIndex = 7;
            this.sousTitre.Text = "                                                   ";
            this.sousTitre.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.sousTitre.Visible = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "a0");
            this.imageList1.Images.SetKeyName(1, "a1");
            this.imageList1.Images.SetKeyName(2, "a2");
            this.imageList1.Images.SetKeyName(3, "a3");
            this.imageList1.Images.SetKeyName(4, "a4");
            this.imageList1.Images.SetKeyName(5, "a5");
            this.imageList1.Images.SetKeyName(6, "a6");
            this.imageList1.Images.SetKeyName(7, "a7");
            this.imageList1.Images.SetKeyName(8, "a8");
            this.imageList1.Images.SetKeyName(9, "a9");
            this.imageList1.Images.SetKeyName(10, "a10");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 563);
            this.Controls.Add(this.splitContainer2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "Opéras";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitFichiers.Panel1.ResumeLayout(false);
            this.splitFichiers.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitFichiers)).EndInit();
            this.splitFichiers.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabFichiers.ResumeLayout(false);
            this.tabOpéras.ResumeLayout(false);
            this.tabMusiciens.ResumeLayout(false);
            this.tabRoles.ResumeLayout(false);
            this.tabDirection.ResumeLayout(false);
            this.tabMise.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.positionSlider)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel nombreOpéras;
        private System.Windows.Forms.ToolStripButton refresh;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripLabel achetés;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TabPage tabOpéras;
        private System.Windows.Forms.TabPage tabMusiciens;
        private System.Windows.Forms.TabPage tabFichiers;
        private System.Windows.Forms.SplitContainer splitFichiers;
        private System.Windows.Forms.TreeView arbreFichiers;
        private System.Windows.Forms.TreeView arbreMusiciens;
        private System.Windows.Forms.TabPage tabDirection;
        private System.Windows.Forms.TreeView arbreDirection;
        private System.Windows.Forms.TabPage tabMise;
        private System.Windows.Forms.TreeView arbreMise;
        private System.Windows.Forms.TabPage tabRoles;
        private System.Windows.Forms.TreeView arbreRoles;
        private System.Windows.Forms.ToolStripLabel Longueur;
        private System.Windows.Forms.Label position2;
        private System.Windows.Forms.MaskedTextBox position1;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label sousTitre;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Controles.Dial volumeDial;
        private Controles.Dial balanceDial;
        private Controles.CustomSlider positionSlider;
        private Controles.CustomButton next;
        private Controles.CustomButton prev;
        private Controles.CustomButton pause;
        private System.Windows.Forms.Button Audio;
        private System.Windows.Forms.TreeView arbreOpéras;
        private Controles.CustomButton zoomInButton;
        private Controles.CustomButton zoomOutButton;
        private System.Windows.Forms.ToolStripButton hide;
        private System.Windows.Forms.Label volume;
        private System.Windows.Forms.ToolStripButton traductionSousTitre;
        private System.Windows.Forms.ToolStripButton Nouveau;
        private System.Windows.Forms.ToolStripComboBox titre;
        private System.Windows.Forms.RichTextBox Argument;
        private System.Windows.Forms.ToolStripComboBox chanteurs;
        private System.Windows.Forms.ImageList imageList1;
        private Controles.CustomButton sound;
    }
}

