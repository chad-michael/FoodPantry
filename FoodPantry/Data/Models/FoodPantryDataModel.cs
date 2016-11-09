namespace FoodPantry.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public partial class FoodPantryDataModel : DbContext
    {
        public FoodPantryDataModel()
            : base("name=FoodPantryRemote")
        {
        }

        public virtual DbSet<EmailSetting> EmailSettings { get; set; }
        public virtual DbSet<EnrollmentStatus> EnrollmentStatus { get; set; }
        public virtual DbSet<FoodPantryLog> FoodPantryLogs { get; set; }
        public virtual DbSet<FPService> FPServices { get; set; }
        public virtual DbSet<IncomeSource> IncomeSources { get; set; }
        public virtual DbSet<InfoSource> InfoSources { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<SentNotice> SentNotices { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<StudentType> StudentTypes { get; set; }
        public virtual DbSet<VisitLog> VisitLogs { get; set; }
        public virtual DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodPantryLog>()
                .Property(e => e.StudentIDNO)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.LocationDesc)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.FoodPantryLogs)
                .WithOptional(e => e.Location)
                .HasForeignKey(e => e.LocationID);

            modelBuilder.Entity<SentNotice>()
                .Property(e => e.IDNO)
                .IsUnicode(false);

            modelBuilder.Entity<StudentType>()
                .HasMany(e => e.FoodPantryLogs)
                .WithOptional(e => e.StudentType1)
                .HasForeignKey(e => e.StudentType);

            modelBuilder.Entity<Note>()
                .Property(e => e.StudentIDNO)
                .IsUnicode(false);

            modelBuilder.Entity<Note>()
                .Property(e => e.Notes)
                .IsUnicode(false);
        }
    }
}