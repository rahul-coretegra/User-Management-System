using Microsoft.EntityFrameworkCore;
using User_Management_System.ManagementConfigurations;

namespace User_Management_System.MicrosoftSqlServerConfigurations
{
    public class MicrosoftSqlServerApplicationDbContext : DbContext
    {
        public MicrosoftSqlServerApplicationDbContext(DbContextOptions<MicrosoftSqlServerApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
