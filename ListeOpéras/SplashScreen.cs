using System.Windows.Forms;

namespace ListeOpéras
{
    public partial class SplashScreen : Form
    {
        public ProgressBar ProgressBar
        {
            get { return this.progressBar; }
            set { this.progressBar = value; }
        }
        public SplashScreen()
        {
            InitializeComponent();
        }
    }
}
