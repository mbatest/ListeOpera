namespace ListeOpéras
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LienYouTube")]
    public partial class LienYouTube
    {
        [Key]
        public int Code_Lien { get; set; }

        public int? Code_Disque { get; set; }

        [StringLength(255)]
        public string YouTube { get; set; }

        public virtual Disques Disques { get; set; }
    }
}
