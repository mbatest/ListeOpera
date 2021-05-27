namespace ListeOpéras
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Définition
    {
        public Définition()
        {
            Disques = new HashSet<Disques>();
        }

        [Key]
        public int Code_Définion { get; set; }
        [StringLength(255)]
        public string Détail { get; set; }
        [StringLength(255)]
        public string Commentaire { get; set; }
        public virtual ICollection<Disques> Disques { get; set; }
    }
}
