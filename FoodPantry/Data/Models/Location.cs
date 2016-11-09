namespace FoodPantry.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Location")]
    public partial class Location
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            FoodPantryLogs = new HashSet<FoodPantryLog>();
        }

        [Key]
        [Column(Order = 0)]
        public byte LocationID { get; set; }

        [Required]
        [StringLength(30)]
        [Column(Order = 1)]
        public string LocationDesc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FoodPantryLog> FoodPantryLogs { get; set; }
    }
}