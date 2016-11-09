namespace FoodPantry.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class oldEmailSetting
    {
        [Key]
        public int EmailID { get; set; }

        [Required]
        [StringLength(2500)]
        public string EmailContents { get; set; }

        [Required]
        [StringLength(100)]
        public string SubjectContents { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }

        public bool ActiveEmail { get; set; }

        public bool Deleted { get; set; }
    }
}