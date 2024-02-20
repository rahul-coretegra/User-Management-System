using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MicrosoftSqlServerRepository;
using User_Management_System.MicrosoftSqlServerRepository.IMSSqlServerRepository;
using User_Management_System.MongoDbConfigurations;
using User_Management_System.MongoDbRepository;
using User_Management_System.MongoDbRepository.IMongoRepository;
using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlRepository;
using User_Management_System.PostgreSqlRepository.IPsqlRepository;

namespace User_Management_System.ManagementModels
{
    public class Instance
    {
        public PostgreSqlApplicationDbContext psqlDbContext {  get; set; }
        
        public IPsqlUnitOfWork psqlUnitOfWork { get; set; }

        public MicrosoftSqlServerApplicationDbContext mssqlDbContext { get; set; }

        public IMsSqlUnitOfWork mssqlUnitOfWork { get; set; }

        public MongoDbApplicationDbContext mongoDbContext { get; set; }

        public IMongoUnitOfWork mongoUnitOfWork { get; set; }
    }
}
