using System;
using System.Windows.Forms;
using PVS.MediaPlayer;
using System.IO;
using System.Text;

namespace Controles
{
    public partial class Visionneuse : Form
    {
        readonly Player player = new Player();
        bool enPause = false;
        readonly AudioDevice[] audioDevices;
        public static string BOM = Encoding.Unicode.GetString(Encoding.Unicode.GetPreamble());
        public Visionneuse(string Filename)
        {
            InitializeComponent();
            player = new Player(panneau);
            player.Events.MediaPositionChanged += Events_MediaPositionChanged;
            player.Events.MediaEnded += Events_MediaEnded;
            player.Sliders.Position.TrackBar = positionSlider;
            sousTitre.Visible = false;

            audioDevices = player.Audio.GetDevices();
            player.Audio.Device = audioDevices[1];
            volumeDial.Value = 500;
            player.Audio.Volume = volumeDial.Value;
            volumeDial.ValueChanged += VolumeDial_ValueChanged;
            balanceDial.ValueChanged += BalanceDial_ValueChanged;
            zoomInButton.MouseWheel += ZoomInButton_MouseWheel;
            zoomOutButton.MouseWheel += ZoomInButton_MouseWheel;
            player.Play(Filename.Replace(BOM, ""), panneau);
            if (Filename.EndsWith("mp4"))
            {
                string f = Filename.Replace("mp4", "srt");
                if (File.Exists(f))
                {
                    player.Subtitles.FileName = f;
                    player.Events.MediaSubtitleChanged += Events_MediaSubtitleChanged;
                    sousTitre.Visible = true;

                }
            }
            if (player.LastError)
            {
                MessageBox.Show(player.GetErrorString(player.LastErrorCode));
                Console.WriteLine(player.GetErrorString(player.LastErrorCode));
            }
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
            sousTitre.Left = (panneau.Width - sousTitre.Width) / 2;
        }
        private void Pause_Click(object sender, EventArgs e)
        {
            player.Paused = !player.Paused;

            enPause = !enPause;
            if (enPause) player.Pause();
            else player.Resume();
        }
        private void Next_Click(object sender, EventArgs e)
        {
            player.Position.FromBegin += TimeSpan.FromSeconds(1);
        }
        private void Prev_Click(object sender, EventArgs e)
        {
            player.Position.FromBegin -= TimeSpan.FromSeconds(1);
        }
        private void Visionneuse_Resize(object sender, EventArgs e)
        {
            sousTitre.Left = (panneau.Width - sousTitre.Width) / 2;
            Refresh();
        }

        private void Audio_Click(object sender, EventArgs e)
        {
            if (player.Audio.Device.Name == "Speakers")
            {
                player.Audio.Device = audioDevices[0];
            }
            else player.Audio.Device = audioDevices[1];
            Audio.Text = player.Audio.Device.Name;
        }

        private void VolumeDial_ValueChanged(object sender, EventArgs e)
        {
            player.Audio.Volume = volumeDial.Value * 0.1f;
        }
        private void BalanceDial_ValueChanged(object sender, Dial.ValueChangedEventArgs e)
        {
            player.Audio.Balance = (e.Value * 0.002f) - 1.0f;
        }
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

        private void VolumeDial_ValueChanged(object sender, Dial.ValueChangedEventArgs e)
        {

        }
    }
}
