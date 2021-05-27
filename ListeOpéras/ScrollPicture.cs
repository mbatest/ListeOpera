using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace ListeOpéras
{
    public partial class ScrollPicture : UserControl
    {
        public event EventHandler Changed;
        public Image Image;
        bool mouvement;
        Point origin = Point.Empty;
        Point départ = Point.Empty;
        float zoom = 1;
        byte[] buffer;
        Point startSelection;
        double Contrast = 1;
        Rectangle selection = Rectangle.Empty;
        public ScrollPicture()
        {
            InitializeComponent();
            MouseWheel += ScrollPicture_MouseWheel;
            MouseDown += ScrollPicture_MouseDown;
        }
        void ScrollPicture_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (e.Delta > 0)
                {
                    Contrast += 0.05f;
                }
                else
                    Contrast -= 0.05f;

                SetContrast(Contrast, ref Image);
            }
            if (e.Delta > 0)
            {
                if (zoom < 5) zoom += 0.05f;
            }
            else
            {

                if (zoom > 0.2) zoom -= 0.05f;
            }
            Refresh();
        }
        private void ScrollPicture_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Black, Bounds);// new Rectangle(0, 0, 80, 80));
            if (Image != null)
            {
                //  zoom = 1;
                Size newSize = new Size((int)(Bounds.Width * zoom),(int)( Bounds.Height *zoom));
                try
                {
                    Bitmap bmp = new Bitmap(Image, newSize);
                    e.Graphics.DrawImage(bmp, origin);
                    bmp.Dispose();
                }
                catch { }
            }
            if (selection.Width > 0 & selection.Height > 0)
            {
                Pen p = new Pen(Brushes.Red, 3)
                {
                    DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
                };
                e.Graphics.DrawRectangle(p, selection);
            }
        }
        private void ScrollPicture_MouseDown(object sender, MouseEventArgs e)
        {
            if (!selection.Contains(e.Location))
                selection = Rectangle.Empty;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (selection == Rectangle.Empty)
                {
                    selection = new Rectangle(e.Location, new Size(0, 0));
                    startSelection = e.Location;
                }
                else
                {
                    if (!selection.Contains(e.Location))
                        selection = Rectangle.Empty;
                    Refresh();
                }
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextMenu cm = new ContextMenu();
                    string[] menuText = new string[] { "Copier", "Couper", "Coller", "Supprimer" };
                    foreach (string mt in menuText)
                    {
                        MenuItem mi = new MenuItem(mt);
                        mi.Click += Mi_Click;
                        cm.MenuItems.Add(mi);
                    }
                    cm.Show(this, this.PointToClient(Cursor.Position));
                }
                else
                {
                    mouvement = true;
                    Cursor = Cursors.Hand;
                    départ = e.Location;
                }
            }
            Refresh();
        }
        void Mi_Click(object sender, EventArgs e)
        {
            switch (((MenuItem)sender).Text)
            {
                case "Coller":
                    Coller(sender, e);
                    break;
                case "Copier":
                    Copier(sender, e);
                    break;
                case "Couper":
                    Copier(sender, e);
                    this.Image = null;
                    Refresh();
                    break;
                case "Supprimer":
                    this.Image = null;
                    Refresh();
                    break;

            }
        }
        private void Copier(object sender, EventArgs e)
        {
            Image im = Image;
            if (im != null)
                if (selection == Rectangle.Empty)
                    Clipboard.SetDataObject(im);
                else
                {

                    Size newSize = new Size((int)(Image.Width * zoom), (int)(Image.Height * zoom));
                    Bitmap bmp = new Bitmap(Image, newSize);
                    using (Graphics gr = Graphics.FromImage(bmp))
                    {
                        gr.DrawImage(bmp, origin);
                    }
                    Bitmap bm = new Bitmap(selection.Width, selection.Height);

                    // Copy the selected area into the bitmap.
                    using (Graphics gr = Graphics.FromImage(bm))
                    {
                        Rectangle dest_rect =
                              new Rectangle(0, 0, selection.Width, selection.Height);
                        gr.DrawImage(bmp, dest_rect, selection,
                            GraphicsUnit.Pixel);
                    }
                    // Copy the selection image to the clipboard.
                    Clipboard.SetImage(bm);
                }
        }
        private void Coller(object sender, EventArgs e)
        {
            DataObject o = (DataObject)Clipboard.GetDataObject();
            String[] arrayOfFormats = o.GetFormats(false);
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Bitmap))
            {
                Image = (Bitmap)Clipboard.GetDataObject().GetData(DataFormats.Bitmap);
                Changed?.Invoke(this, new EventArgs());
            }
            if (Clipboard.ContainsImage())
            {
                Image = Clipboard.GetImage();
                Changed?.Invoke(this, new EventArgs());
                Refresh();
            }
        }
        public void SetImage(Image im, bool nu)
        {
            origin = System.Drawing.Point.Empty;
            Image = im;
            if (im != null)
            {
                zoom = 1;
                origin = Point.Empty;
                if (im != null)
                {
                    //vScrollBar1.Maximum = im.Height;
                    //hScrollBar1.Maximum = im.Width;
                }
            }
            Refresh();
        }
        public void SetImage(byte[] buffer)
        {
            origin = System.Drawing.Point.Empty;
            if (buffer != null)
            {
                this.buffer = buffer;
                MemoryStream ms = new MemoryStream(buffer);
                Image = System.Drawing.Image.FromStream(ms);
            }
            else
                Image = null;
            zoom = 1;
            origin = Point.Empty;

            if (Image != null)
            {
                //vScrollBar1.Maximum = Image.Height;
                //hScrollBar1.Maximum = Image.Width;
            }
            if (Changed != null)
                Changed(this, new EventArgs());
            Refresh();
        }
        public void SetImageF(string image, bool locked)
        {
            if (String.IsNullOrEmpty(image)) return;
            if (!String.IsNullOrEmpty(image))
            {
                MemoryStream ms = new MemoryStream(new WebClient().DownloadData(image));
                Image = System.Drawing.Image.FromStream(ms);
            }
            //          Image = Image.FromFile(image);
            origin = Point.Empty;
           if (Image != null)
            {
                //vScrollBar1.Maximum = Image.Height;
                //hScrollBar1.Maximum = Image.Width;
                if (!locked)
                {
                    zoom = (float)Height / Image.Height;
                    origin = Point.Empty;
                }
            }
            selection = Rectangle.Empty;
            startSelection = Point.Empty;
            Refresh();
        }
        public byte[] GetImage()
        {
            if (Image != null)
            {
                MemoryStream ms = new MemoryStream();
                Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
            return null;
        }
        private void ScrollPicture_MouseHover(object sender, EventArgs e)
        {
            //http://www.codeproject.com/Articles/21097/PictureBox-Zoom
        }
        private void ScrollPicture_MouseMove(object sender, MouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (startSelection != System.Drawing.Point.Empty)
                    selection = new Rectangle(startSelection, new Size(e.Location.X - startSelection.X, e.Location.Y - startSelection.Y));
                Refresh();
            }
            else
            if (mouvement)
            {
                int x = e.Location.X - départ.X;
                int y = e.Location.Y - départ.Y;
                //     if (x < Image.Width) { }
                départ = e.Location;
                origin.Offset(x, y);
                Refresh();
            }
        }
        private void ScrollPicture_MouseUp(object sender, MouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
            }
            else if (mouvement)
            {
                mouvement = false;
                Cursor = Cursors.Default;
            }
        }
        public void SetContrast(double contrast, ref Image _currentBitmap)
        {
            Bitmap temp = new Bitmap(_currentBitmap);
            Bitmap bmap = (Bitmap)temp.Clone();
            if (contrast < -100) contrast = -100;
            if (contrast > 100) contrast = 100;
            contrast = (100.0 + contrast) / 100.0;
            contrast *= contrast;
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    double pR = Contraste(contrast, c.R / 255.0);
                    double pG = Contraste(contrast, c.G / 255.0);
                    double pB = Contraste(contrast, c.B / 255.0);
                    bmap.SetPixel(i, j, Color.FromArgb((byte)pR, (byte)pG, (byte)pB));
                }
            }
            _currentBitmap = (Bitmap)bmap.Clone();
        }
        private static double Contraste(double contrast, double pR)
        {
            pR -= 0.5;
            pR *= contrast;
            pR += 0.5;
            pR *= 255;
            if (pR < 0) pR = 0;
            if (pR > 255) pR = 255;
            return pR;
        }
    }

}
