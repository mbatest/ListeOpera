namespace ListeOpéras
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Disques
    {
        public Disques()
        {
            Diriger = new HashSet<Diriger>();
            Interprétation = new HashSet<Interprétation>();
            LienYouTube = new HashSet<LienYouTube>();
            MiseEnScene = new HashSet<MiseEnScene>();
            Marqueurs = new HashSet<Marqueurs>();
        }
        [Key]
        public int Code_Disque { get; set; }
        public int? Code_Opéra { get; set; }
        public int? Code_Editeur { get; set; }
        public int? Code_Salle { get; set; }
        public int? Code_Définition { get; set; }
        public int? Année { get; set; }
        public int? Durée { get; set; }
        [StringLength(255)]
        public string Format { get; set; }
        [StringLength(255)]
        public string Vidéo { get; set; }
        [StringLength(10)]
        public string Texte_Définition { get; set; }
        public virtual Définition Définition { get; set; }
        [StringLength(10)]
        public string Audio { get; set; }
        [Column(TypeName = "image")]
        public byte[] Pochette { get; set; }
        public virtual Editeur Editeur { get; set; }
        public virtual Opéra Opéra { get; set; }
        public virtual Salles Salles { get; set; }
        public int? Nombre_Disques { get; set; }
        [StringLength(25)]
        public string ASIN { get; set; }
        [StringLength(255)]
        public string Source { get; set; }
        [StringLength(255)]
        public string URL { get; set; }
        [StringLength(255)]
        public string Fichier { get; set; }
        public string Notes { get; set; }
        public decimal? Prix { get; set; }
        public virtual ICollection<Diriger> Diriger { get; set; }
        public virtual ICollection<Interprétation> Interprétation { get; set; }
        public virtual ICollection<LienYouTube> LienYouTube { get; set; }
        public virtual ICollection<MiseEnScene> MiseEnScene { get; set; }
        public virtual ICollection<Marqueurs> Marqueurs { get; set; }
    }
}
