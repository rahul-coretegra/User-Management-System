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
        public DbSet<Service> Services { get; set; }
        public DbSet<ConfigureService> ConfigureServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigureService>()
                .HasOne(ura => ura.Service)
                .WithMany()
                .HasForeignKey(ura => ura.ServiceUniqueId);

        }
    }
}
