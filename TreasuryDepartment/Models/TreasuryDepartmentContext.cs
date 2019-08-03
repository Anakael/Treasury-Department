using Microsoft.EntityFrameworkCore;

namespace TreasuryDepartment.Models
{
    public class TreasuryDepartmentContext : DbContext
    {
        public TreasuryDepartmentContext(DbContextOptions<TreasuryDepartmentContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Balance> Balances { get; set; }
        public DbSet<BlackList> BlackLists { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Balance>(entity =>
            {
                entity.HasKey(t => new {t.SenderUserId, t.TargetUserId});

                entity.HasOne(d => d.SenderUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.SenderUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TargetUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.TargetUserId);
            });

            modelBuilder.Entity<BlackList>(entity =>
            {
                entity.HasKey(t => new {t.SenderUserId, t.TargetUserId});

                entity.HasOne(bl => bl.SenderUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.SenderUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bl => bl.TargetUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.TargetUserId);
            });

            modelBuilder.Entity<Deal>(entity =>
            {
                entity.HasKey(t => new {t.SenderUserId, t.TargetUserId});

                entity.HasOne(d => d.SenderUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.SenderUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TargetUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.TargetUserId);
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.HasKey(t => new {t.SenderUserId, t.TargetUserId});

                entity.HasOne(f => f.SenderUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.SenderUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.TargetUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.TargetUserId);
            });
        }
    }
}