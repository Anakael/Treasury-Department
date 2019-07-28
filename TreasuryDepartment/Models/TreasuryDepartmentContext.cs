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
		public DbSet<Invite> Invites { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Balance>(entity =>
			{
				entity.HasKey(t => new { t.SourceUserId, t.TargetUserId });

				entity.HasOne(d => d.SourceUser)
					.WithMany()
					.HasForeignKey(ug => ug.SourceUserId)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(d => d.TargetUser)
					.WithMany()
					.HasForeignKey(ug => ug.TargetUserId);

			});

			modelBuilder.Entity<BlackList>(entity =>
			{
				entity.HasKey(t => new { t.SenderUserId, t.TargetUserId });

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
				entity.HasKey(t => new { t.SenderUserId, t.TargetUserId });

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
				entity.HasKey(t => new { t.User1Id, t.User2Id });

				entity.HasOne(f => f.User1)
					.WithMany()
					.HasForeignKey(ug => ug.User1Id)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(f=> f.User2)
					.WithMany()
					.HasForeignKey(ug => ug.User2Id);
			});

			modelBuilder.Entity<Invite>(entity =>
			{
				entity.HasKey(i => new { i.SenderUserId, i.TargetUserId });
				entity.HasOne(i => i.SenderUser)
					.WithMany()
					.HasForeignKey(ug => ug.SenderUserId)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(i=> i.TargetUser)
					.WithMany()
					.HasForeignKey(ug => ug.TargetUserId);
			});
		}
	}
}