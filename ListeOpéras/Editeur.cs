namespace ListeOpéras
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Editeur")]
    public partial class Editeur
    {
        public Editeur()
        {
            Disques = new HashSet<Disques>();
        }

        [Key]
        public int Code_Editeur { get; set; }
        [Column("Editeur")]
        [StringLength(255)]
        public string NomEditeur { get; set; }
        public int? Code_Pays { get; set; }
        public virtual ICollection<Disques> Disques { get; set; }
        public virtual Pays Pays { get; set; }
    }
}
