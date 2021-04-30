namespace ListeOpéras
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Instrument")]
    public partial class Instrument
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Instrument()
        {
            Musicien = new HashSet<Musicien>();
        }

        [Key]
        public int Code_Instrument { get; set; }

        [StringLength(50)]
        public string Nom_Instrument { get; set; }

        [Column(TypeName = "image")]
        public byte[] Image { get; set; }
        public string Type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Musicien> Musicien { get; set; }
    }
}
