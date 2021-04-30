namespace ListeOpéras
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Disques
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Disques()
        {
            Diriger = new HashSet<Diriger>();
            Interprétation = new HashSet<Interprétation>();
            LienYouTube = new HashSet<LienYouTube>();
            MiseEnScene = new HashSet<MiseEnScene>();
        }

        [Key]
        public int Code_Disque { get; set; }

        public int? Code_Opéra { get; set; }

        public int? Code_Editeur { get; set; }

        public int? Code_Salle { get; set; }

        [StringLength(255)]
        public string Format { get; set; }

        [StringLength(255)]
        public string Vidéo { get; set; }

        [StringLength(255)]
        public string Audio { get; set; }

        public int? Année { get; set; }

        public int? Durée { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }
        public string Comment { get; set; }
        public int? NombreDisque { get; set; }

        [StringLength(255)]
        public string URL { get; set; }
        [StringLength(255)]
        public string Fichier { get; set; }

        public int? Stock { get; set; }

        public decimal? Prix { get; set; }

        [StringLength(25)]
        public string ASIN { get; set; }

        [Column(TypeName = "image")]
        public byte[] Pochette { get; set; }

        [StringLength(255)]
        public string Lien_Pochette { get; set; }

        public decimal? Prix_payé { get; set; }

        public int? Checked { get; set; }

        [StringLength(255)]
        public string Source { get; set; }

        [StringLength(255)]
        public string Définition { get; set; }

        public int? Détail_Définition { get; set; }

        public virtual Définition Définition1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Diriger> Diriger { get; set; }

        public virtual Editeur Editeur { get; set; }

        public virtual Opéra Opéra { get; set; }

        public virtual Salles Salles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Interprétation> Interprétation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LienYouTube> LienYouTube { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MiseEnScene> MiseEnScene { get; set; }
    }
}
