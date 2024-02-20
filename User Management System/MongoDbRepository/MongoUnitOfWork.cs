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
            UserAndRoles = new UserAndRolesRepository(_context);
            RoleAndAccess = new RoleAndAccessRepository(_context);
            Menus = new MenuRepository(_context);
            RoleAndMenus = new RoleAndMenusRepository(_context);
        }

        public IUserRoleRepository UserRoles { private set; get; }

        public IUserRepository Users { private set; get; }

        public IRouteRepository Routes { private set; get; }

        public IUserAndRolesRepository UserAndRoles { private set; get; }

        public IRoleAndAccessRepository RoleAndAccess { private set; get; }

        public IMenuRepository Menus { private set; get; }

        public IRoleAndMenusRepository RoleAndMenus { private set; get; }

    }
}
