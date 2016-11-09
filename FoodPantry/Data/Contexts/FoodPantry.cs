using FoodPantry.Data.Models;
using System.Data.Entity;

namespace FoodPantry.Data.Contexts
{
    public partial class FoodPantry : DbContext
    {
        static FoodPantry()
        {
            Database.SetInitializer<FoodPantry>(null);
        }

        public FoodPantry()
            : base("name=FoodPantryTest")
        {
        }

        public virtual DbSet<EmailSetting> EmailSettings { get; set; }
        public virtual DbSet<EnrollmentStatus> EnrollmentStatus { get; set; }
        public virtual DbSet<FoodPantryLog> FoodPantryLogs { get; set; }
        public virtual DbSet<FpService> FpServices { get; set; }
        public virtual DbSet<IncomeSource> IncomeSources { get; set; }
        public virtual DbSet<InfoSource> InfoSources { get; set; }
        public virtual DbSet<SentNotice> SentNotices { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<StudentType> StudentTypes { get; set; }
        public virtual DbSet<VisitLog> VisitLogs { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodPantryLog>()
                .Property(e => e.StudentIdno)
                .IsUnicode(false);

            modelBuilder.Entity<SentNotice>()
                .Property(e => e.Idno)
                .IsUnicode(false);

            modelBuilder.Entity<StudentType>()
                .HasMany(e => e.FoodPantryLogs)
                .WithOptional(e => e.StudentType1)
                .HasForeignKey(e => e.StudentType);

            modelBuilder.Entity<Location>()
                .Property(e => e.LocationDesc)
                .IsUnicode(false);

            modelBuilder.Entity<Note>()
                .Property(e => e.StudentIdno)
                .IsUnicode(false);

            modelBuilder.Entity<Note>()
                .Property(e => e.Notes)
                .IsUnicode(false);
        }
    }
}