using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EmployeesCrud.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<EmployeeMaster> EmployeeMaster { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Country Configuration
        //    modelBuilder.Entity<Country>(entity =>
        //    {
        //        entity.HasKey(e => e.Row_Id);
        //        entity.Property(e => e.CountryName).IsRequired().HasMaxLength(50);
        //    });

        //    // State Configuration
        //    modelBuilder.Entity<State>(entity =>
        //    {
        //        entity.HasKey(e => e.Row_Id);
        //        entity.Property(e => e.StateName).IsRequired().HasMaxLength(50);
        //        entity.HasOne(e => e.Country)
        //              .WithMany(c => c.States)
        //              .HasForeignKey(e => e.CountryId);
        //    });

        //    // City Configuration
        //    modelBuilder.Entity<City>(entity =>
        //    {
        //        entity.HasKey(e => e.Row_Id);
        //        entity.Property(e => e.CityName).IsRequired().HasMaxLength(50);
        //        entity.HasOne(e => e.State)
        //              .WithMany(s => s.Cities)
        //              .HasForeignKey(e => e.StateId);
        //    });

        //    // EmployeeMaster Configuration
        //    modelBuilder.Entity<EmployeeMaster>(entity =>
        //    {
        //        entity.HasKey(e => e.Row_Id);
        //        entity.Property(e => e.EmployeeCode).IsRequired().HasMaxLength(8);
        //        entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
        //        entity.Property(e => e.LastName).HasMaxLength(50);
        //        entity.Property(e => e.EmailAddress).IsRequired().HasMaxLength(100);
        //        entity.Property(e => e.MobileNumber).IsRequired().HasMaxLength(15);
        //        entity.Property(e => e.PanNumber).IsRequired().HasMaxLength(12);
        //        entity.Property(e => e.PassportNumber).IsRequired().HasMaxLength(20);
        //        entity.Property(e => e.ProfileImage).HasMaxLength(100);
        //        entity.Property(e => e.DateOfBirth).IsRequired();
        //        //entity.Property(e => e.CreatedDate).IsRequired().HasDefaultValueSql("GETUTCDATE()");
        //        entity.Property(e => e.IsActive).IsRequired();
        //        //entity.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);

        //        entity.HasOne(e => e.Country)
        //              .WithMany()
        //              .HasForeignKey(e => e.CountryId);

        //        entity.HasOne(e => e.State)
        //              .WithMany()
        //              .HasForeignKey(e => e.StateId);

        //        entity.HasOne(e => e.City)
        //              .WithMany()
        //              .HasForeignKey(e => e.CityId);
        //        entity.ToTable("EmployeeMaster");
        //    });
        //}
    }
}