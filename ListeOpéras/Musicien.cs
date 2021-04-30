namespace ListeOpéras
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Musicien")]
    public partial class Musicien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Musicien()
        {
            Diriger = new HashSet<Diriger>();
            Interprétation = new HashSet<Interprétation>();
            MiseEnScene = new HashSet<MiseEnScene>();
            Opéra = new HashSet<Opéra>();
            Opéra1 = new HashSet<Opéra>();
        }

        [Key]
        public int Code_Musicien { get; set; }

        [StringLength(200)]
        public string Nom_Musicien { get; set; }

        [StringLength(50)]
        public string Prénom_Musicien { get; set; }

        public int? Année_Naissance { get; set; }

        public int? Année_Mort { get; set; }

        public int? Code_Pays { get; set; }

        public int? Code_Instrument { get; set; }
        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }

        public int? Checked { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Diriger> Diriger { get; set; }

        public virtual Instrument Instrument { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Interprétation> Interprétation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MiseEnScene> MiseEnScene { get; set; }
        public virtual Pays Pays { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Opéra> Opéra { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Opéra> Opéra1 { get; set; }
    }
}
