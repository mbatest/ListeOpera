namespace ListeOpéras
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Diriger")]
    public partial class Diriger
    {
        [Key]
        public int Code_Diriger { get; set; }
        public int Code_Disque { get; set; }
        public int? Code_Musicien { get; set; }
        public int Code_Orchestre { get; set; }
        public virtual Disques Disques { get; set; }
        public virtual Musicien Musicien { get; set; }
        public virtual Orchestres Orchestres { get; set; }
    }
}
