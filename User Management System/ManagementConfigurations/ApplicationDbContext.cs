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

    }
}
