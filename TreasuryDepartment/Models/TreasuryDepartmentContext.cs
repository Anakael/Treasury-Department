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
					.WithMany(su => su.OutcomeBalances)
					.HasForeignKey(ug => ug.SourceUserId)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(d => d.TargetUser)
					.WithMany(su => su.IncomeBalances)
					.HasForeignKey(ug => ug.TargetUserId);

			});

			modelBuilder.Entity<BlackList>(entity =>
			{
				entity.HasKey(t => new { t.SenderUserId, t.TargetUserId });

				entity.HasOne(f => f.SenderUser)
					.WithMany(su => su.OutcomeBlackLists)
					.HasForeignKey(ug => ug.SenderUserId)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(bl => bl.TargetUser)
					.WithMany(su => su.IncomeBlackLists)
					.HasForeignKey(ug => ug.TargetUserId);
			});

			modelBuilder.Entity<Deal>(entity =>
			{
				entity.HasKey(t => new { t.SenderUserId, t.TargetUserId });

				entity.HasOne(d => d.SenderUser)
					.WithMany(su => su.SentDeals)
					.HasForeignKey(ug => ug.SenderUserId)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(d => d.TargetUser)
					.WithMany(su => su.ReciviedDeals)
					.HasForeignKey(ug => ug.TargetUserId);
			});

			modelBuilder.Entity<Friend>(entity =>
			{
				entity.HasKey(t => new { t.User1Id, t.User2Id });

				entity.HasOne(f => f.User1)
					.WithMany(su => su.OutcomeFriends)
					.HasForeignKey(ug => ug.User1Id)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(f => f.User2)
					.WithMany(su => su.IncomeFriends)
					.HasForeignKey(ug => ug.User2Id);
			});

			modelBuilder.Entity<Invite>(entity =>
			{
				entity.HasKey(i => new { i.SenderUserId, i.TargetUserId });
				entity.HasOne(d => d.SenderUser)
					.WithMany(su => su.SentInvites)
					.HasForeignKey(ug => ug.SenderUserId)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(d => d.TargetUser)
					.WithMany(su => su.ReciviedInvites)
					.HasForeignKey(ug => ug.TargetUserId);
			});
		}
	}
}