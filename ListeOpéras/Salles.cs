namespace ListeOpéras
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Salles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Salles()
        {
            Disques = new HashSet<Disques>();
        }

        [Key]
        public int Code_Salle { get; set; }

        [StringLength(255)]
        public string Nom_Salle { get; set; }

        [StringLength(255)]
        public string Ville { get; set; }

        public int? Code_Pays { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Disques> Disques { get; set; }

        public virtual Pays Pays { get; set; }
    }
}
