namespace FoodPantry.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VisitLog")]
    public partial class oldVisitLog
    {
        public int VisitLogID { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
    }
}