namespace FoodPantry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EmailSetting
    {
        [Key]
        public int EmailID { get; set; }

        [Required]
        [StringLength(2500)]
        [DisplayName("Email Contents")]
        public string EmailContents { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Email Subject")]
        public string SubjectContents { get; set; }

        [DisplayName("Date Added")]
        public DateTime DateAdded { get; set; }

        [DisplayName("Date Modified")]
        public DateTime DateModified { get; set; }

        [DisplayName("Date Deleted")]
        public DateTime? DateDeleted { get; set; }

        [DisplayName("Is Active Email?")]
        public bool ActiveEmail { get; set; }

        [DisplayName("Is Deleted?")]
        public bool Deleted { get; set; }
    }
}
