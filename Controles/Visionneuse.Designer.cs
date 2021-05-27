namespace Controles

{
    partial class Visionneuse
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
            }
            base.Dispose(disposing);
            player.Dispose();
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.panneau = new System.Windows.Forms.Panel();
            this.sousTitre = new System.Windows.Forms.Label();
            this.position1 = new System.Windows.Forms.Label();
            this.position2 = new System.Windows.Forms.Label();
            this.Audio = new Controles.CustomButton();
            this.zoomInButton = new Controles.CustomButton();
            this.zoomOutButton = new Controles.CustomButton();
            this.next = new Controles.CustomButton();
            this.prev = new Controles.CustomButton();
            this.pause = new Controles.CustomButton();
            this.balanceDial = new Controles.Dial();
            this.volumeDial = new Controles.Dial();
            this.positionSlider = new Controles.CustomSlider();
            this.panneau.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // panneau
            // 
            this.panneau.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panneau.Controls.Add(this.sousTitre);
            this.panneau.Location = new System.Drawing.Point(131, 3);
            this.panneau.Name = "panneau";
            this.panneau.Size = new System.Drawing.Size(585, 535);
            this.panneau.TabIndex = 0;
            // 
            // sousTitre
            // 
            this.sousTitre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sousTitre.AutoSize = true;
            this.sousTitre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.sousTitre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(173)))), ((int)(((byte)(143)))));
            this.sousTitre.Location = new System.Drawing.Point(167, 495);
            this.sousTitre.Name = "sousTitre";
            this.sousTitre.Size = new System.Drawing.Size(264, 20);
            this.sousTitre.TabIndex = 7;
            this.sousTitre.Text = "                                                   ";
            this.sousTitre.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // position1
            // 
            this.position1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.position1.AutoSize = true;
            this.position1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.position1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(173)))), ((int)(((byte)(146)))));
            this.position1.Location = new System.Drawing.Point(143, 585);
            this.position1.Name = "position1";
            this.position1.Size = new System.Drawing.Size(55, 24);
            this.position1.TabIndex = 1;
            this.position1.Text = "         ";
            // 
            // position2
            // 
            this.position2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.position2.AutoSize = true;
            this.position2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.position2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(173)))), ((int)(((byte)(146)))));
            this.position2.Location = new System.Drawing.Point(839, 583);
            this.position2.Name = "position2";
            this.position2.Size = new System.Drawing.Size(45, 24);
            this.position2.TabIndex = 2;
            this.position2.Text = "       ";
            // 
            // Audio
            // 
            this.Audio.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Audio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(176)))), ((int)(((byte)(143)))));
            this.Audio.Location = new System.Drawing.Point(795, 537);
            this.Audio.Name = "Audio";
            this.Audio.Size = new System.Drawing.Size(75, 23);
            this.Audio.TabIndex = 24;
            this.Audio.Text = "Audio";
            this.Audio.UseVisualStyleBackColor = true;
            this.Audio.Click += new System.EventHandler(this.Audio_Click);
            // 
            // zoomInButton
            // 
            this.zoomInButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.zoomInButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.zoomInButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.zoomInButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zoomInButton.Font = new System.Drawing.Font("Webdings", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.zoomInButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(176)))), ((int)(((byte)(143)))));
            this.zoomInButton.Location = new System.Drawing.Point(84, 585);
            this.zoomInButton.Name = "zoomInButton";
            this.zoomInButton.Size = new System.Drawing.Size(28, 24);
            this.zoomInButton.TabIndex = 23;
            this.zoomInButton.Text = "";
            this.zoomInButton.UseVisualStyleBackColor = true;
            this.zoomInButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ZoomInButton_MouseDown);
            // 
            // zoomOutButton
            // 
            this.zoomOutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.zoomOutButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.zoomOutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zoomOutButton.Font = new System.Drawing.Font("Webdings", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.zoomOutButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(176)))), ((int)(((byte)(143)))));
            this.zoomOutButton.Location = new System.Drawing.Point(109, 585);
            this.zoomOutButton.Name = "zoomOutButton";
            this.zoomOutButton.Size = new System.Drawing.Size(28, 24);
            this.zoomOutButton.TabIndex = 22;
            this.zoomOutButton.Text = "";
            this.zoomOutButton.UseVisualStyleBackColor = true;
            this.zoomOutButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ZoomOutButton_MouseDown);
            // 
            // next
            // 
            this.next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.next.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.next.Font = new System.Drawing.Font("Webdings", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.next.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(176)))), ((int)(((byte)(143)))));
            this.next.Location = new System.Drawing.Point(56, 585);
            this.next.Name = "next";
            this.next.Size = new System.Drawing.Size(28, 24);
            this.next.TabIndex = 21;
            this.next.Text = "";
            this.next.UseVisualStyleBackColor = true;
            this.next.Click += new System.EventHandler(this.Next_Click);
            // 
            // prev
            // 
            this.prev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.prev.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.prev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.prev.Font = new System.Drawing.Font("Webdings", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.prev.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(176)))), ((int)(((byte)(143)))));
            this.prev.Location = new System.Drawing.Point(29, 585);
            this.prev.Name = "prev";
            this.prev.Size = new System.Drawing.Size(28, 24);
            this.prev.TabIndex = 20;
            this.prev.Text = "";
            this.prev.UseVisualStyleBackColor = true;
            this.prev.Click += new System.EventHandler(this.Prev_Click);
            // 
            // pause
            // 
            this.pause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pause.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pause.Font = new System.Drawing.Font("Webdings", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.pause.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(176)))), ((int)(((byte)(143)))));
            this.pause.Location = new System.Drawing.Point(2, 585);
            this.pause.Name = "pause";
            this.pause.Size = new System.Drawing.Size(28, 24);
            this.pause.TabIndex = 19;
            this.pause.Text = ";";
            this.pause.UseVisualStyleBackColor = true;
            this.pause.Click += new System.EventHandler(this.Pause_Click);
            // 
            // balanceDial
            // 
            this.balanceDial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.balanceDial.Image = global::Controles.Properties.Resources.Dial_Normal_2;
            this.balanceDial.Location = new System.Drawing.Point(73, 527);
            this.balanceDial.MaximumSize = new System.Drawing.Size(55, 55);
            this.balanceDial.MinimumSize = new System.Drawing.Size(55, 55);
            this.balanceDial.Name = "balanceDial";
            this.balanceDial.Size = new System.Drawing.Size(55, 55);
            this.balanceDial.TabIndex = 16;
            this.balanceDial.Text = "Balance";
            this.balanceDial.Value = 0;
            // 
            // volumeDial
            // 
            this.volumeDial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.volumeDial.Image = global::Controles.Properties.Resources.Dial_Normal_2;
            this.volumeDial.Location = new System.Drawing.Point(13, 527);
            this.volumeDial.MaximumSize = new System.Drawing.Size(55, 55);
            this.volumeDial.MinimumSize = new System.Drawing.Size(55, 55);
            this.volumeDial.Name = "volumeDial";
            this.volumeDial.Size = new System.Drawing.Size(55, 55);
            this.volumeDial.TabIndex = 15;
            this.volumeDial.Text = "Volume";
            this.volumeDial.Value = 0;
            this.volumeDial.ValueChanged += new System.EventHandler<Controles.Dial.ValueChangedEventArgs>(this.VolumeDial_ValueChanged);
            // 
            // positionSlider
            // 
            this.positionSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.positionSlider.AutoSize = false;
            this.positionSlider.Location = new System.Drawing.Point(139, 586);
            this.positionSlider.Name = "positionSlider";
            this.positionSlider.Size = new System.Drawing.Size(573, 23);
            this.positionSlider.TabIndex = 3;
            this.positionSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // Visionneuse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(882, 610);
            this.Controls.Add(this.Audio);
            this.Controls.Add(this.zoomInButton);
            this.Controls.Add(this.zoomOutButton);
            this.Controls.Add(this.next);
            this.Controls.Add(this.prev);
            this.Controls.Add(this.pause);
            this.Controls.Add(this.balanceDial);
            this.Controls.Add(this.volumeDial);
            this.Controls.Add(this.positionSlider);
            this.Controls.Add(this.position2);
            this.Controls.Add(this.position1);
            this.Controls.Add(this.panneau);
            this.Name = "Visionneuse";
            this.Resize += new System.EventHandler(this.Visionneuse_Resize);
            this.panneau.ResumeLayout(false);
            this.panneau.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panneau;
        private System.Windows.Forms.Label position1;
        private System.Windows.Forms.Label position2;
        private CustomSlider positionSlider;
        private System.Windows.Forms.Label sousTitre;
        private Dial balanceDial;
        private Dial volumeDial;
        private CustomButton next;
        private CustomButton prev;
        private CustomButton pause;
        private CustomButton zoomInButton;
        private CustomButton zoomOutButton;
        private CustomButton Audio;
    }
}
