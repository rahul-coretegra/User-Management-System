using Microsoft.EntityFrameworkCore;
using User_Management_System.ManagementModels;
using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlRepository;
using User_Management_System.ManagementConfigurations;
using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MongoDbConfigurations;
using Microsoft.Extensions.Options;
using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.MicrosoftSqlServerRepository;

namespace User_Management_System.ManagementRepository
{
    public class DbContextConfigurations:IDbContextConfigurations
    {
        private readonly ApplicationDbContext _context;
        private PostgreSqlApplicationDbContext _psqlcontext;
        private MicrosoftSqlServerApplicationDbContext _mssqlcontext;
        private readonly MongoDbApplicationDbContext _mongodbcontext;
        private readonly MongoDbSettings _mongodbsettings;
        private readonly IConfiguration _configuration;
        private readonly IOptions<AppSettings> _appsettings;

        public DbContextConfigurations(ApplicationDbContext context, IConfiguration configuration,
            PostgreSqlApplicationDbContext postgreSql, MicrosoftSqlServerApplicationDbContext microsoftSql,
            MongoDbApplicationDbContext mongoDb, IOptions<MongoDbSettings> mongodbsettings,
            IOptions<AppSettings> settings)
        {
            _context = context;
            _configuration = configuration;
            _psqlcontext = postgreSql;
            _mssqlcontext = microsoftSql;
            _mongodbcontext = mongoDb;
            _mongodbsettings = mongodbsettings.Value;
            _appsettings = settings;
        }

        public bool establishDbConnection(Project Project)
        {
            if (Project == null)
                return false;

            else if (Project.TypeOfDatabase == TypeOfDatabase.PostgreSql)
            {
                var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlApplicationDbContext>();

                optionsBuilder.UseNpgsql(Project.ConnectionString);

                _psqlcontext = new PostgreSqlApplicationDbContext(optionsBuilder.Options);

                _psqlcontext.Database.Migrate();

                return true;

            }
            else if (Project.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
            {
                var optionsBuilder = new DbContextOptionsBuilder<MicrosoftSqlServerApplicationDbContext>();

                optionsBuilder.UseSqlServer(Project.ConnectionString);

                _mssqlcontext = new MicrosoftSqlServerApplicationDbContext(optionsBuilder.Options);

                _mssqlcontext.Database.Migrate();

                return true;

            }
            else if (Project.TypeOfDatabase == TypeOfDatabase.MongoDb)
            {
                _mongodbsettings.MongoDbConnection = _configuration.GetSection("MongoDbConfigurations")["MongoDbConnection"] = Project.ConnectionString;
                _mongodbsettings.DatabaseName = _configuration.GetSection("MongoDbConfigurations")["DatabaseName"] = Project.DatabaseName;

                return true;
            }
            else
                return false;
        }



        public Instance configureDbContext(Project Project)
        {

            Instance instances = new Instance();
            if (Project.TypeOfDatabase == TypeOfDatabase.PostgreSql)
            {
                var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlApplicationDbContext>();

                var connectionString = _configuration.GetSection("PostgreSqlConfigurations")["PostgreSqlConnectionString"];

                connectionString = Project.ConnectionString;

                optionsBuilder.UseNpgsql(connectionString);

                _psqlcontext = new PostgreSqlApplicationDbContext(optionsBuilder.Options);

                instances.psqlDbContext = _psqlcontext;

                instances.psqlUnitOfWork = new PsqlUnitOfWork(_psqlcontext, _appsettings);

            }
            else if (Project.TypeOfDatabase == TypeOfDatabase.MicrosoftSqlServer)
            {
                var optionsBuilder = new DbContextOptionsBuilder<MicrosoftSqlServerApplicationDbContext>();

                var connectionString = _configuration.GetSection("MicrosoftSqlServerConfigurations")["MicrosoftSqlServerConnection"];

                connectionString = Project.ConnectionString;

                optionsBuilder.UseSqlServer(connectionString);

                _mssqlcontext = new MicrosoftSqlServerApplicationDbContext(optionsBuilder.Options);

                instances.mssqlDbContext = _mssqlcontext;

                instances.mssqlUnitOfWork = new MsSqlUnitOfWork(_mssqlcontext, _appsettings);

            }



            return instances;
        }
    }
}
