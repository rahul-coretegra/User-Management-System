using Microsoft.EntityFrameworkCore;
using User_Management_System.PostgreSqlModels;
using User_Management_System.PostgreSqlModels.SupremeModels;

namespace User_Management_System.PostgreSqlConfigurations
{
    public class PostgreSqlApplicationDbContext : DbContext
    {
        public PostgreSqlApplicationDbContext(DbContextOptions<PostgreSqlApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserAndRoles> UserAndUserRoles { get; set; }

        public DbSet<PostgreSqlModels.Route> Routes { get; set; }

        public DbSet<RoleAndAccess> RoleAndAccess { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserAndRoles>()
                .HasOne(ura => ura.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ura => ura.UserId);

            modelBuilder.Entity<UserAndRoles>()
                .HasOne(ura => ura.UserRole)
                .WithMany()
                .HasForeignKey(ura => ura.RoleId);


            modelBuilder.Entity<RoleAndAccess>()
                .HasOne(ura => ura.UserRole)
                .WithMany()
                .HasForeignKey(ura => ura.RoleId);

            modelBuilder.Entity<RoleAndAccess>()
                .HasOne(ura => ura.Route)
                .WithMany()
                .HasForeignKey(ura => ura.RouteId);
        }
    }
}
