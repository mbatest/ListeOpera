using PVS.MediaPlayer;
using System.Windows.Forms;

namespace ListeOpéras
{
    public partial class Pays
    {
        public override string ToString()
        {
            return Nom_Pays;
        }
    }
    public partial class Disques
    {
        public string FullPath ;
        public void PlayVideo(Player player, Panel panneau )
        {
            player.Play(FullPath, panneau);
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
            return Editeur1;
        }
    }
    public partial class Roles
    {
        public override string ToString()
        {
            return Role + "(" + Voix + ")";
        }
    }
    public partial class Musicien
    {
        public override string ToString()
        {
            return Nom_Musicien + ", " + Prénom_Musicien;
        }
    }
    public partial class Opéra
    {
        public override string ToString()
        {
            return Titre + " (" + Musicien.Nom_Musicien + ", " + Musicien.Prénom_Musicien + " " + Année.ToString() + ")";
        }
        public string Affiche
        {
            get { return ToString(); }
        }
    }
}
