namespace ListeOpéras
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Editeur")]
    public partial class Editeur
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Editeur()
        {
            Disques = new HashSet<Disques>();
        }

        [Key]
        public int Code_Editeur { get; set; }

        [Column("Editeur")]
        [StringLength(255)]
        public string Editeur1 { get; set; }

        public int? Code_Pays { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Disques> Disques { get; set; }

        public virtual Pays Pays { get; set; }
    }
}
