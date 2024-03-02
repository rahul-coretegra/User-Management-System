using Microsoft.Extensions.Options;
using User_Management_System.MongoDbConfigurations;
using User_Management_System.MongoDbRepository.IMongoRepository;

namespace User_Management_System.MongoDbRepository
{
    public class MongoUnitOfWork :IMongoUnitOfWork
    {
        private readonly MongoDbApplicationDbContext _context;
        private readonly IOptions<AppSettings> _appsettings;

        public MongoUnitOfWork(MongoDbApplicationDbContext dbContext, IOptions<AppSettings> settings) 
        {
            _context = dbContext;
            _appsettings = settings;
            UserRoles = new UserRoleRepository(_context);
            Users = new UserRepository(_context,_appsettings);
            Routes = new RouteRepository(_context);
            RoleAccess = new RoleAccessRepository(_context);
            RouteAccess = new RouteAccessRepository(_context);
            Menus = new MenuRepository(_context);
            MenuAccess = new MenuAccessRepository(_context);
            Services = new ServiceRepository(_context);
            ConfigureServices = new ConfigureServiceRepository(_context);


        }

        public IUserRoleRepository UserRoles { private set; get; }

        public IUserRepository Users { private set; get; }

        public IRouteRepository Routes { private set; get; }

        public IRoleAccessRepository RoleAccess { private set; get; }

        public IRouteAccessRepository RouteAccess { private set; get; }

        public IMenuRepository Menus { private set; get; }

        public IMenuAccessRepository MenuAccess { private set; get; }

        public IServiceRepository Services { private set; get; }

        public IConfigureServiceRepository ConfigureServices { private set; get; }



    }
}
