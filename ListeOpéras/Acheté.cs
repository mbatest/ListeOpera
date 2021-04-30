namespace ListeOpéras
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Acheté
    {
        public int ID { get; set; }

        [StringLength(255)]
        public string URL_Achat { get; set; }

        [Column(TypeName = "money")]
        public decimal? Prix { get; set; }
    }
}
