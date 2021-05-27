using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace ListeOpéras
{
    [Table("Marqueurs")]
    public partial class Marqueurs
    {
        public Marqueurs()
        {
        }
        [Key]
        public int Code_Marqueur { get; set; }
        public int? Code_Disque { get; set; }
        public int Code_Air { get; set; }
        public long? Adresse { get; set; }
        public string Date { get; set; }
 //       public string Nom_Marqueur { get; set; }
        public virtual Disques  Disques { get; set; }
        public virtual Airs Airs { get; set; }
    }
    [Table("Airs")]
    public partial class Airs
    {
        public Airs()
        {
        }
        [Key]
        public int Code_Air { get; set; }
        public int Code_Opéra { get; set; }
        [StringLength(150)]
        public string Nom { get; set; }
        public virtual Opéra Opéra { get; set; }
    }
    public class ASIN
    {
        public string Asin { get; set; }
        public override string ToString()
        {
            return Asin;
        }
    }
}
