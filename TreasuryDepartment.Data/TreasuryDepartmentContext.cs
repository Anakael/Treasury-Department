using Microsoft.EntityFrameworkCore;
using TreasureDepartment.Data.Dbo;

namespace TreasureDepartment.Data
{
    public class TreasuryDepartmentContext : DbContext
    {
        public TreasuryDepartmentContext(DbContextOptions<TreasuryDepartmentContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<BlackListDbo> BlackLists { get; set; }
        public DbSet<DealDbo> Deals { get; set; }
        public DbSet<FriendInviteDbo> Friends { get; set; }
        public DbSet<UserDbo> Users { get; set; }
        public DbSet<TokenDbo> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlackListDbo>(entity =>
            {
                entity.HasKey(t => new {t.SenderUserId, t.TargetUserId});

                entity.HasOne(bl => bl.SenderUserDbo)
                    .WithMany()
                    .HasForeignKey(ug => ug.SenderUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bl => bl.TargetUserDbo)
                    .WithMany()
                    .HasForeignKey(ug => ug.TargetUserId);
            });

            modelBuilder.Entity<DealDbo>(entity =>
            {
                entity.HasKey(t => new {t.SenderUserId, t.TargetUserId});

                entity.HasOne(d => d.SenderUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.SenderUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TargetUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.TargetUserId);

                entity.Property(d => d.Sum).HasColumnType("decimal(6, 2)");
            });

            modelBuilder.Entity<FriendInviteDbo>(entity =>
            {
                entity.HasKey(t => new {t.SenderUserId, t.TargetUserId});

                entity.HasOne(f => f.SenderUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.SenderUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.TargetUser)
                    .WithMany()
                    .HasForeignKey(ug => ug.TargetUserId);

                entity.Property(f => f.Sum).HasColumnType("decimal(6, 2)");
            });

            modelBuilder.Entity<UserDbo>(entity => { entity.HasIndex(u => u.Login).IsUnique(); });
            modelBuilder.Entity<TokenDbo>(entity => { entity.HasKey(t => new {t.UserId, t.Token, t.RefreshToken}); });
        }
    }
}