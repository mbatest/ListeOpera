namespace ListeOpéras
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("YoutubeAsin")]
    public partial class YoutubeAsin
    {
        public int ID { get; set; }

        [StringLength(255)]
        public string Auteur { get; set; }

        [StringLength(255)]
        public string Titre { get; set; }

        [StringLength(255)]
        public string ASIN { get; set; }

        [StringLength(255)]
        public string Youtube { get; set; }
    }
}
