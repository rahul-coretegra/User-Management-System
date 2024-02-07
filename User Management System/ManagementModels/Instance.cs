using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MicrosoftSqlServerRepository;
using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlRepository;

namespace User_Management_System.ManagementModels
{
    public class Instance
    {
        public PostgreSqlApplicationDbContext psqlDbContext {  get; set; }
        
        public PsqlUnitOfWork psqlUnitOfWork { get; set; }

        public MicrosoftSqlServerApplicationDbContext mssqlDbContext { get; set; }

        public MsSqlUnitOfWork mssqlUnitOfWork { get; set; }
    }
}
