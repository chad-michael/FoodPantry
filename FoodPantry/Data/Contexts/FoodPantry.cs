namespace FoodPantry.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FoodPantry : DbContext
    {
        static FoodPantry()
        {
            Database.SetInitializer<FoodPantry>(null);
        }


        public FoodPantry()
            : base("name=FoodPantry")
        {
        }

        public virtual DbSet<EmailSetting> EmailSettings { get; set; }
        public virtual DbSet<EnrollmentStatu> EnrollmentStatus { get; set; }
        public virtual DbSet<FoodPantryLog> FoodPantryLogs { get; set; }
        public virtual DbSet<FPService> FPServices { get; set; }
        public virtual DbSet<IncomeSource> IncomeSources { get; set; }
        public virtual DbSet<InfoSource> InfoSources { get; set; }
        public virtual DbSet<SentNotice> SentNotices { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<StudentType> StudentTypes { get; set; }
        public virtual DbSet<VisitLog> VisitLogs { get; set; }
        public virtual DbSet<Note> Notes { get; set; }

    }
}
