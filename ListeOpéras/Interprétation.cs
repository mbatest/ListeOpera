namespace ListeOpéras
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Interprétation
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Code_Disque { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Code_Musicien { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Code_Role { get; set; }

        [StringLength(255)]
        public string Commentaire { get; set; }

        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }

        public virtual Disques Disques { get; set; }

        public virtual Musicien Musicien { get; set; }

        public virtual Roles Roles { get; set; }
    }
}
