namespace ListeOpéras
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MiseEnScene")]
    public partial class MiseEnScene
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Code_Disque { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Code_Musicien { get; set; }

        [StringLength(50)]
        public string Salle { get; set; }

        public virtual Disques Disques { get; set; }

        public virtual Musicien Musicien { get; set; }
    }
}
