using System.ComponentModel;

namespace FoodPantry.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FoodPantryLog")]
    public partial class FoodPantryLog
    {
        public int FoodPantryLogID { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 1)]
        public string StudentIDNO { get; set; }

        [StringLength(100, MinimumLength = 1)]
        [DisplayName("Student Name")]
        public string StudentName { get; set; }

        [DisplayName("Date Inserted")]
        public DateTime? DateInserted { get; set; }

        [Range(0, short.MaxValue)]
        [DisplayName("# In Household")]
        public short? NoInHousehold { get; set; }

        [DisplayName("# of Delta Students")]
        [Range(0, short.MaxValue)]
        public short? DeltaStudCount { get; set; }

        [DisplayName("# of Children 0 to 5")]
        [Range(0, short.MaxValue)]
        public short? NoChildren0Thru5 { get; set; }

        [DisplayName("# of Children 6 to 18")]
        [Range(0, short.MaxValue)]
        public short? NoChildren6Thru8 { get; set; }

        [DisplayName("Student Type")]
        public short? StudentType { get; set; }

        [DisplayName("Enrollment Status")]
        public int? EnrollmentStatusID { get; set; }

        [DisplayName("Info Sources")]
        public int? InfoSourcesID { get; set; }

        [StringLength(200)]
        [DisplayName("Explain Other")]
        public string ExplainOther { get; set; }

        [Required]
        [StringLength(100)]
        public string Signature { get; set; }

        [DisplayName("Income Loans?")]
        public bool? Inc_Loans { get; set; }

        [DisplayName("Income Spouse?")]
        public bool? Inc_Spouse { get; set; }

        [DisplayName("Income On Campus?")]
        public bool? Inc_OnCampus { get; set; }

        [DisplayName("Income Off Campus?")]
        public bool? Inc_OffCampus { get; set; }

        [DisplayName("Income Scholarships?")]
        public bool? Inc_Scholarships { get; set; }

        [DisplayName("Income Parental Support?")]
        public bool? Inc_ParentalSupport { get; set; }

        [DisplayName("Income Other?")]
        public bool? Inc_Other { get; set; }

        [DisplayName("Income Grants?")]
        public bool? Inc_Grants { get; set; }

        [Required]
        [DisplayName("# Large Bags")]
        public short? Qty_LargeBag { get; set; }

        [Required]
        [DisplayName("# Small Bags")]
        public short? Qty_SmallBag { get; set; }

        [DisplayName("Water Bottle")]
        public DateTime? WaterBottle { get; set; }

        [DisplayName("Campus Location of Intake")]
        public byte? LocationID { get; set; }

        [DisplayName("Enrollment Status")]
        public virtual EnrollmentStatus EnrollmentStatus { get; set; }

        [DisplayName("Info Sources")]
        public virtual InfoSource InfoSource { get; set; }

        [DisplayName("Location of Intake")]
        [ForeignKey("LocationID")]
        public virtual Location Location { get; set; }

        [DisplayName("Student Type")]
        [ForeignKey("StudentType")]
        public virtual StudentType StudentType1 { get; set; }
    }
}