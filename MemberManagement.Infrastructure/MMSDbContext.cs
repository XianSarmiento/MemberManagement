using MemberManagement.Domain;
using MemberManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberManagement.Infrastructure
{
    public class MMSDbContext : DbContext
    {
        public MMSDbContext(DbContextOptions<MMSDbContext> options) : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration for Member
            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.BirthDate)
                      .HasColumnType("date");
            });

            // Configuration for MembershipType
            modelBuilder.Entity<MembershipType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MembershipCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.Property(e => e.MembershipFee).HasColumnType("decimal(18,2)");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            base.OnModelCreating(modelBuilder);

            // Relationships and Constraints for Member Entity
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasOne(m => m.Branch)
                      .WithMany()
                      .HasForeignKey(m => m.BranchId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.MembershipType)
                      .WithMany()
                      .HasForeignKey(m => m.MembershipTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
