namespace FoodPantry.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class oldNote
    {
        [Key]
        [Column(Order = 0)]
        public int NoteID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string StudentIDNO { get; set; }

        [Column(TypeName = "text")]
        public string Notes { get; set; }

        public DateTime? Updated { get; set; }
    }
}