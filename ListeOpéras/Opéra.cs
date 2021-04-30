namespace ListeOpéras
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Opéra
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Opéra()
        {
            Disques = new HashSet<Disques>();
            Roles = new HashSet<Roles>();
            Musicien1 = new HashSet<Musicien>();
        }

        [Key]
        public int Code_Opéra { get; set; }

        [StringLength(255)]
        public string Opus { get; set; }

        [StringLength(255)]
        public string Titre { get; set; }

        [StringLength(255)]
        public string Acts { get; set; }

        public int? Année { get; set; }

        public int? Code_Musicien { get; set; }

        [Column(TypeName = "ntext")]
        public string Argument { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Disques> Disques { get; set; }

        public virtual Musicien Musicien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Roles> Roles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Musicien> Musicien1 { get; set; }
    }
}
