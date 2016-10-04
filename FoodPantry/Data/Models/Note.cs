namespace FoodPantry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Note
    {
        public int NoteID { get; set; }

        [Required]
        [StringLength(10)]
        public string StudentIDNO { get; set; }

        [Column(TypeName = "text")]
        public string Notes { get; set; }

        public DateTime? Updated { get; set; }
    }
}
