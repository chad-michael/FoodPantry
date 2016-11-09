namespace FoodPantry.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StudentType")]
    public partial class oldStudentType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public oldStudentType()
        {
            FoodPantryLogs = new HashSet<oldFoodPantryLog>();
        }

        [Key]
        [Column("StudentType")]
        public short StudentType1 { get; set; }

        [StringLength(100)]
        public string StudentTypeDesc { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }

        public bool? Active { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<oldFoodPantryLog> FoodPantryLogs { get; set; }
    }
}
