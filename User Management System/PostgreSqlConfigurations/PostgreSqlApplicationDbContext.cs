using Microsoft.EntityFrameworkCore;
using User_Management_System.PostgreSqlModels;

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
        
        public DbSet<Menu> Menus { get; set; }

        public DbSet<RoleAndMenus> RoleAndMenus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserAndRoles>()
                .HasOne(ura => ura.User)
                .WithMany()
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

            modelBuilder.Entity<RoleAndMenus>()
                .HasOne(ura => ura.UserRole)
                .WithMany()
                .HasForeignKey(ura => ura.RoleId);

            modelBuilder.Entity<RoleAndMenus>()
                .HasOne(ura => ura.Menu)
                .WithMany()
                .HasForeignKey(ura => ura.MenuId);
        }
    }
}
