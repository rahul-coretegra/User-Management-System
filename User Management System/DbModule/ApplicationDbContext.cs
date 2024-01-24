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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasOne(c => c.userRole)
            .WithMany()
            .HasForeignKey(c => c.roleUniqueCode);
        }
    }
}
