using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PVS.MediaPlayer;

namespace ListeOpéras
{
    public partial class MainForm
    {
        Player player;
        AudioDevice[] audioDevices;

        public void Init(string Filename, Panel panneau)
        {
            splitContainer1.Panel2Collapsed = false;
            if (player != null)
                player.Dispose();
            player = new Player(panneau);
            player.Events.MediaPositionChanged += Events_MediaPositionChanged;
            player.Events.MediaEnded += Events_MediaEnded;
            player.Sliders.Position.TrackBar = positionSlider;
            sousTitre.Visible = false;

            audioDevices = player.Audio.GetDevices();
            player.Audio.Device = audioDevices[1];
            Audio.Text = player.Audio.Device.Name;
            volumeDial.ValueChanged += VolumeDial_ValueChanged;
            balanceDial.ValueChanged += BalanceDial_ValueChanged;
            volumeDial.Value = 50;
            zoomInButton.MouseWheel += ZoomInButton_MouseWheel;
            zoomOutButton.MouseWheel += ZoomInButton_MouseWheel;
            if (player != null)
            {
                player.Play(Filename.Replace(MainForm.BOM, ""), panel1);
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
            }
            if (player.LastError)
            {
                MessageBox.Show(player.GetErrorString(player.LastErrorCode));
                Console.WriteLine(player.GetErrorString(player.LastErrorCode));
            }
        }
        private void BalanceDial_ValueChanged(object sender, Controles.Dial.ValueChangedEventArgs e)
        {
            player.Audio.Balance = (e.Value * 0.002f) - 1.0f;
        }
        private void VolumeDial_ValueChanged(object sender, Controles.Dial.ValueChangedEventArgs e)
        {
            player.Audio.Volume = e.Value * 0.001f;
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
            sousTitre.Left = /*panel1.Left +*/ (panel1.Width - sousTitre.Width) / 2;
        }
        private void Pause_Click(object sender, EventArgs e)
        {
            player.Paused = !player.Paused;
        }
        private void Next_Click(object sender, EventArgs e)
        {
            player.Position.FromBegin += TimeSpan.FromSeconds(1);
        }
        private void Prev_Click(object sender, EventArgs e)
        {
            player.Position.FromBegin -= TimeSpan.FromSeconds(1);
        }
        private void Audio_Click(object sender, EventArgs e)
        {
            //        if (player.Audio.Device == null)
            if (player.Audio.Device.Name == "Speakers")
            {
                player.Audio.Device = audioDevices[0];
            }
            else player.Audio.Device = audioDevices[1];
            Audio.Text = player.Audio.Device.Name;
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
    }
}
