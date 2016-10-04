namespace FoodPantry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Setting
    {
        [Key]
        public int SettingsID { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(100)]
        public string SettingsValue { get; set; }

        [StringLength(500)]
        public string MultiLineSettingValue { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }

        public bool? Active { get; set; }
    }
}
