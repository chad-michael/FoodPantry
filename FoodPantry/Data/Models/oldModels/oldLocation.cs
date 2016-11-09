namespace FoodPantry.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Location")]
    public partial class oldLocation
    {
        [Key]
        [Column(Order = 0)]
        public byte LocationID { get; set; }

        [Column(Order = 1)]
        [StringLength(30)]
        public string LocationDesc { get; set; }

        public virtual ICollection<oldFoodPantryLog> FoodPantryLogs { get; set; }
    }
}