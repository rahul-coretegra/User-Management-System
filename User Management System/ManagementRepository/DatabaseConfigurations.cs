using Microsoft.EntityFrameworkCore;
using User_Management_System.PostgreSqlConfigurations;

using User_Management_System.ManagementConfigurations;
using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MongoDbConfigurations;
using Microsoft.Extensions.Options;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.MicrosoftSqlServerRepository;
using User_Management_System.MongoDbRepository;
using User_Management_System.PostgreSqlRepository;


namespace User_Management_System.ManagementRepository
{
    public class DatabaseConfigurations : IDbContextConfigurations
    {
        private readonly ApplicationDbContext _context;
        private PostgreSqlApplicationDbContext _psqlcontext;
        private MicrosoftSqlServerApplicationDbContext _mssqlcontext;

        private readonly IOptions<MongoDbSettings> _mongodbsettings;
        private MongoDbApplicationDbContext _mongodbcontext;
        private readonly IOptions<AppSettings> _appsettings;

        public DatabaseConfigurations(ApplicationDbContext context,
            PostgreSqlApplicationDbContext postgreSql, MicrosoftSqlServerApplicationDbContext microsoftSql,
            MongoDbApplicationDbContext mongoDb, IOptions<MongoDbSettings> mongodbsettings,
            IOptions<AppSettings> settings)
        {
            _context = context;
            _psqlcontext = postgreSql;
            _mssqlcontext = microsoftSql;
            _mongodbcontext = mongoDb;
            _mongodbsettings = mongodbsettings;
            _appsettings = settings;
        }

        public bool establishDbConnection(Project Project)
        {
            try
            {
                if (Project.TypeOfDatabase == TypeOfDatabase.PostgreSql)
                {
                    _psqlcontext = configurePostgreSqlDbContext(Project.ConnectionString);

                    if (Project.MigrateDatabase == TrueFalse.True)
                        return MigratePostgreSqlDataBase(_psqlcontext);

                    return true;

                }
                else if (Project.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
                {
                    _mssqlcontext = configureMicrosoftSqlServerDbContext(Project.ConnectionString);

                    if (Project.MigrateDatabase == TrueFalse.True)
                        return MigrateMicrosoftSqlServerDataBase(_mssqlcontext);

                    return true;
                }
                if (Project.TypeOfDatabase == TypeOfDatabase.MongoDb)
                {
                    _mongodbcontext = configureMongoDbDbContext(Project.ConnectionString, Project.DatabaseName);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public PostgreSqlApplicationDbContext configurePostgreSqlDbContext(string ConnectionString)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlApplicationDbContext>();

                optionsBuilder.UseNpgsql(ConnectionString);

                return new PostgreSqlApplicationDbContext(optionsBuilder.Options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public bool MigratePostgreSqlDataBase(PostgreSqlApplicationDbContext applicationDbContext)
        {
            applicationDbContext.Database.Migrate();
            if (applicationDbContext.Database.CanConnect())
                return true;
            return false;
        }

        public MicrosoftSqlServerApplicationDbContext configureMicrosoftSqlServerDbContext(string ConnectionString)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<MicrosoftSqlServerApplicationDbContext>();
                optionsBuilder.UseSqlServer(ConnectionString);
                return new MicrosoftSqlServerApplicationDbContext(optionsBuilder.Options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public bool MigrateMicrosoftSqlServerDataBase(MicrosoftSqlServerApplicationDbContext applicationDbContext)
        {
            applicationDbContext.Database.Migrate();
            if (applicationDbContext.Database.CanConnect())
                return true;
            return false;
        }

        public MongoDbApplicationDbContext configureMongoDbDbContext(string ConnectionString, string DatabaseName)
        {
            try
            {
                _mongodbsettings.Value.Client = ConnectionString;

                _mongodbsettings.Value.DatabaseName = DatabaseName;

                return new MongoDbApplicationDbContext(_mongodbsettings);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public Instance configureDbContexts(Project Project)
        {
            Instance instances = new Instance();
            if (Project.TypeOfDatabase == TypeOfDatabase.PostgreSql)
            {

                instances.psqlDbContext = configurePostgreSqlDbContext(Project.ConnectionString);

                instances.psqlUnitOfWork = new PsqlUnitOfWork(instances.psqlDbContext, _appsettings);

            }
            else if (Project.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
            {

                instances.mssqlDbContext = configureMicrosoftSqlServerDbContext(Project.ConnectionString);

                instances.mssqlUnitOfWork = new MsSqlUnitOfWork(instances.mssqlDbContext, _appsettings);

            }

            else if (Project.TypeOfDatabase == TypeOfDatabase.MongoDb)
            {

                instances.mongoDbContext = configureMongoDbDbContext(Project.ConnectionString, Project.DatabaseName);

                instances.mongoUnitOfWork = new MongoUnitOfWork(instances.mongoDbContext, _appsettings);

            }

            return instances;
        }
    }
}
