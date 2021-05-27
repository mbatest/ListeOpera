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
        public int Code_Mise{ get; set; }
        public int Code_Disque { get; set; }
        public int Code_Musicien { get; set; }

        [StringLength(50)]
        public string Salle { get; set; }

        public virtual Disques Disques { get; set; }

        public virtual Musicien Musicien { get; set; }
    }
}
