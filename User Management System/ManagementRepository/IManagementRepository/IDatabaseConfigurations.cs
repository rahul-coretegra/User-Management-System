using User_Management_System.ManagementModels;
using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MongoDbConfigurations;
using User_Management_System.PostgreSqlConfigurations;

namespace User_Management_System.ManagementRepository.IManagementRepository
{
    public interface IDbContextConfigurations
    {

        bool establishDbConnection(Project Project);


        PostgreSqlApplicationDbContext configurePostgreSqlDbContext(string ConnectionString);

        bool MigratePostgreSqlDataBase(PostgreSqlApplicationDbContext applicationDbContext);


        MicrosoftSqlServerApplicationDbContext configureMicrosoftSqlServerDbContext(string ConnectionString);

        bool MigrateMicrosoftSqlServerDataBase(MicrosoftSqlServerApplicationDbContext applicationDbContext);


        MongoDbApplicationDbContext configureMongoDbDbContext(string ConnectionString, string DatabaseName);

        Instance configureDbContexts(Project Project);
    }
}
