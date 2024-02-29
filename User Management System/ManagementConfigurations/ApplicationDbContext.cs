using Microsoft.EntityFrameworkCore;
using User_Management_System.ManagementModels;

namespace User_Management_System.ManagementConfigurations
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<SupremeUser> SupremeUsers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Service)
                .WithMany()
                .HasForeignKey(i => i.ServiceUniqueId);
        }
    }
}
