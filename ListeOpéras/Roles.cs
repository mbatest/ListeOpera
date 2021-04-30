namespace ListeOpéras
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Roles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Roles()
        {
            Interprétation = new HashSet<Interprétation>();
        }

        [Key]
        public int Code_Role { get; set; }

        public int? Code_Opéra { get; set; }

        [StringLength(255)]
        public string Role { get; set; }

        [StringLength(255)]
        public string Voix { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
        [Column(TypeName = "image")]
        public byte[] Image { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Interprétation> Interprétation { get; set; }

        public virtual Opéra Opéra { get; set; }
    }
}
