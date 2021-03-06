namespace ListeOpéras
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Opéra
    {
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
        public virtual ICollection<Disques> Disques { get; set; }
        public virtual Musicien Musicien { get; set; }
        public string Argument { get; set; }
        public virtual ICollection<Roles> Roles { get; set; }
        public virtual ICollection<Musicien> Musicien1 { get; set; }
    }
}
