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
		public DbSet<Deal> Deals { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<Invite> Invites { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserGroup> UserGroups { get; set; }

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

			modelBuilder.Entity<Invite>(entity =>
			{
				entity.HasKey(i => new { i.UserId, i.GroupId });
			});

			modelBuilder.Entity<UserGroup>(entity =>
			{
				entity.HasKey(t => new { t.UserId, t.GroupId });
			});
		}
	}
}