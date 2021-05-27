using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PVS.MediaPlayer;

namespace ListeOpéras
{
    /// <summary>
    /// Contient la partie visionnage
    /// </summary>
    public partial class MainForm
    {
        private Player player;
        private AudioDevice[] audioDevices;
        private Disques disqueCourant;
        bool soundOn;
        readonly ToolTip pauseToolTip = new ToolTip();
        /// <summary>
        /// Méthode pour visionner un film
        /// </summary>
        /// <param name="panneau">Panneau pour l'affichage</param>
        /// <param name="disque">Disque à visionner</param>
        public void Visionner(Panel panneau, Disques disque)
        {
            splitContainer1.Panel2Collapsed = false;
            splitFichiers.Panel2Collapsed = false;
            disqueCourant = disque;
            if (player != null)
                player.Dispose();
            player = new Player(panneau);
            player.Events.MediaPositionChanged += Events_MediaPositionChanged;
            player.Events.MediaEnded += Events_MediaEnded;
            player.Sliders.Position.TrackBar = positionSlider;
            sousTitre.Visible = false;
            audioDevices = player.Audio.GetDevices();
            if (audioDevices.Count() == 1)
                player.Audio.Device = audioDevices[0];
            else
                player.Audio.Device = audioDevices[1];
            player.Audio.Device = audioDevices.Where(c => c.Name == "Speakers").First();
            Audio.Text = player.Audio.Device.Name;
            volumeDial.ValueChanged += VolumeDial_ValueChanged;
            balanceDial.ValueChanged += BalanceDial_ValueChanged;
            zoomInButton.MouseWheel += ZoomInButton_MouseWheel;
            zoomOutButton.MouseWheel += ZoomInButton_MouseWheel;
            sousTitre.Left = (this.panel.Width - sousTitre.Width) / 2;
            volumeDial.Value = 500;
            sound.Text = "m";
            sound.Font = new Font("Webdings", 14.5f, FontStyle.Bold);
            sound.Font = new Font("Musique", 14.5f, FontStyle.Bold);
            player.Audio.Volume = volumeDial.Value;
            sousTitre.Visible = false;
            if (player != null)
            {
                player.Play(disque.URL/*.Replace(MainForm.BOM, "")*/, this.panel);
                if (disque.URL.EndsWith("mp4"))
                {
                    if (File.Exists(disque.URL.Replace("mp4", "srt")))
                    {
                        player.Subtitles.FileName = disque.URL.Replace("mp4", "srt");
                        player.Events.MediaSubtitleChanged += Events_MediaSubtitleChanged;
                    }
                }
                disque.GetDétails(player);
//                Détails(disque);
                int audio = player.Audio.ChannelCount;

            }
            if (player.LastError)
            {
                MessageBox.Show(player.GetErrorString(player.LastErrorCode));
                Console.WriteLine(player.GetErrorString(player.LastErrorCode));
            }
            pauseToolTip.ToolTipTitle = "";
            pauseToolTip.UseFading = true;
            pauseToolTip.UseAnimation = true;
            pauseToolTip.IsBalloon = false;
            pauseToolTip.ShowAlways = true;
            pauseToolTip.AutoPopDelay = 5000;
            pauseToolTip.InitialDelay = 500;
            pauseToolTip.ReshowDelay = 500;
            pauseToolTip.SetToolTip(sound, "Couper le son");
        }
        private void BalanceDial_ValueChanged(object sender, Controles.Dial.ValueChangedEventArgs e)
        {
            player.Audio.Balance = (e.Value * 0.002f) - 1.0f;
        }
        private void VolumeDial_ValueChanged(object sender, Controles.Dial.ValueChangedEventArgs e)
        {
            player.Audio.Volume = e.Value * 0.001f;
            volume.Text = ((int)(player.Audio.Volume *1000)).ToString();
            soundOn = e.Value > 50;
        }
        private void Events_MediaEnded(object sender, EndedEventArgs e)
        {
            //   throw new NotImplementedException();
        }
        private void Events_MediaPositionChanged(object sender, PositionEventArgs e)
        {
            position1.Text = TimeSpan.FromTicks(e.FromStart).ToString(@"hh\:mm\:ss");
            position2.Text = TimeSpan.FromTicks(e.ToStop).ToString(@"hh\:mm\:ss");
        }
        private void Events_MediaSubtitleChanged(object sender, SubtitleEventArgs e)
        {
            sousTitre.Text = e.Subtitle;
            sousTitre.Visible = true;
            sousTitre.Left =  (panel.Width - sousTitre.Width) / 2;
        }
        private void Pause_Click(object sender, EventArgs e)
        {
            player.Paused = !player.Paused;
            if (player.Paused) pause.Text = "\u0034";
            else pause.Text = "\u003B";// ";";

        }
        #region Modification de la position du media
        private void Position1_Enter(object sender, EventArgs e)
        {
            pause.PerformClick();
            //player.Paused = true;
            //pause.Text = "\u0034";
        }
        private void Position1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (TimeSpan.TryParse(position1.Text, out TimeSpan r))
                {
                    Marqueurs marqueur = new Marqueurs { Disques = disqueCourant, Adresse = r.Ticks, Date= r.ToString() };
                    MiseAJourObjet ms = new MiseAJourObjet(marqueur/*, md*/, disqueCourant.Trouve(arbreFichiers.Nodes));
                    if(ms.ShowDialog()== DialogResult.OK)
                    {
                         disqueCourant.AjoutMarqueur(marqueur, arbreFichiers);

                    }
                }
            }
        }
        private void Position1_Leave(object sender, EventArgs e)
        {
            if (player.Paused)
            {
                pause.PerformClick();
                if (TimeSpan.TryParse(position1.Text, out TimeSpan result))
                {
                    player.Position.FromBegin = result;
                }
                //player.Paused = false;
                //pause.Text = "\u003B";// ";";
            }
        }
        private void Next_Click(object sender, EventArgs e)
        {
            player.Position.FromBegin += TimeSpan.FromSeconds(1);
        }
        private void Prev_Click(object sender, EventArgs e)
        {
            player.Position.FromBegin -= TimeSpan.FromSeconds(1);
        }
        int volumeSon;
        private void Sound_Click(object sender, EventArgs e)
        {
            if (soundOn)
            {
                volumeSon = volumeDial.Value;
                soundOn = false;
                sound.Text = "p";
                volumeDial.Value = 0;
                pauseToolTip.SetToolTip(sound, "Remettre le son");

            }
            else
            {
                volumeDial.Value = volumeSon;
                soundOn = true;
                sound.Text = "m";// "\u0058";
                pauseToolTip.SetToolTip(sound, "Couper le son");

            }
        }
        #endregion
        private void Audio_Click(object sender, EventArgs e)
        {
            if (audioDevices.Count() == 1) return;        
            if (player.Audio.Device.Name == "Speakers")
            {
                player.Audio.Device = audioDevices[0];
            }
            else player.Audio.Device = audioDevices[1];
            Audio.Text = player.Audio.Device.Name;
        }
        #region Zoom
        private void ZoomInButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) player.Video.Zoom(1.1);
        }
        private void ZoomOutButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) player.Video.Zoom(0.9);
        }
        void ZoomInButton_MouseWheel(object sender, MouseEventArgs e)
        {
            player.Video.Zoom(e.Delta > 0 ? 1.1 : 0.9);
        }
        #endregion
    }
}
