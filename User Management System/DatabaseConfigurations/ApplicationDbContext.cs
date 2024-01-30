using Microsoft.EntityFrameworkCore;
using User_Management_System.Models.SupremeModels;
using User_Management_System.Models.UserModels;

namespace User_Management_System.DbModule
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {

        }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAndRoles> UserAndUserRoles { get; set; }

        public DbSet<RoleAndAccess> RoleAndAccess { get; set; }
        public DbSet<UserVerification> UserVerifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAndRoles>()
                .HasKey(ura => ura.userAndRoleUniqueId);

            modelBuilder.Entity<UserAndRoles>()
                .HasOne(ura => ura.user)
                .WithMany(u => u.userRoles)
                .HasForeignKey(ura => ura.userUniqueCode);

            modelBuilder.Entity<UserAndRoles>()
                .HasOne(ura => ura.userRole)
                .WithMany()
                .HasForeignKey(ura => ura.roleUniqueCode);

            modelBuilder.Entity<RoleAndAccess>()
                .HasOne(c => c.userRole)
                .WithMany()
                .HasForeignKey(c => c.roleUniqueCode);
        }
    }
}
