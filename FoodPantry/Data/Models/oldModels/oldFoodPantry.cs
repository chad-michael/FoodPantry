using System.Data.Entity;

namespace FoodPantry.Data.Models
{
    public partial class oldFoodPantry : DbContext
    {
        static oldFoodPantry()
        {
            Database.SetInitializer<oldFoodPantry>(null);
        }

        public oldFoodPantry() : base("name=FoodPantryTest")
        {
        }

        public virtual DbSet<oldEmailSetting> EmailSettings { get; set; }
        public virtual DbSet<oldEnrollmentStatus> EnrollmentStatus { get; set; }
        public virtual DbSet<oldFoodPantryLog> FoodPantryLogs { get; set; }
        public virtual DbSet<oldFPService> FPServices { get; set; }
        public virtual DbSet<oldIncomeSource> IncomeSources { get; set; }
        public virtual DbSet<oldInfoSource> InfoSources { get; set; }
        public virtual DbSet<oldSentNotice> SentNotices { get; set; }
        public virtual DbSet<oldSetting> Settings { get; set; }
        public virtual DbSet<oldStudentType> StudentTypes { get; set; }
        public virtual DbSet<oldVisitLog> VisitLogs { get; set; }
        public virtual DbSet<oldLocation> Locations { get; set; }
        public virtual DbSet<oldNote> Notes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<oldFoodPantryLog>()
                .Property(e => e.StudentIDNO)
                .IsUnicode(false);

            modelBuilder.Entity<oldSentNotice>()
                .Property(e => e.IDNO)
                .IsUnicode(false);

            modelBuilder.Entity<oldStudentType>()
                .HasMany(e => e.FoodPantryLogs)
                .WithOptional(e => e.StudentType1)
                .HasForeignKey(e => e.StudentType);

            modelBuilder.Entity<oldLocation>()
                .HasMany(e => e.FoodPantryLogs)
                .WithOptional(e => e.LocationDesc)
                .HasForeignKey(e => e.LocationID);

            modelBuilder.Entity<oldNote>()
                .Property(e => e.StudentIDNO)
                .IsUnicode(false);

            modelBuilder.Entity<oldNote>()
                .Property(e => e.Notes)
                .IsUnicode(false);
        }
    }
}