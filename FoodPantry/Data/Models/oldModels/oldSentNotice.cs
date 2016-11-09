namespace FoodPantry.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SentNotice")]
    public partial class oldSentNotice
    {
        public int SentNoticeID { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(10)]
        public string IDNO { get; set; }

        public DateTime? DateSent { get; set; }

        public string MessageContent { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }

        public bool? Active { get; set; }
    }
}