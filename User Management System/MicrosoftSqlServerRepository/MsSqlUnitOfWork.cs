using Microsoft.Extensions.Options;
using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MicrosoftSqlServerRepository.IMSSqlServerRepository;
using User_Management_System.PostgreSqlConfigurations;

namespace User_Management_System.MicrosoftSqlServerRepository
{
    public class MsSqlUnitOfWork: IMsSqlUnitOfWork
    {
        private readonly MicrosoftSqlServerApplicationDbContext _context;
        private readonly IOptions<AppSettings> _appsettings;

        public MsSqlUnitOfWork(MicrosoftSqlServerApplicationDbContext context, IOptions<AppSettings> settings)
        {
            _context = context;
            _appsettings = settings;
            UserRoles = new UserRoleRepository(_context);
            Users = new UserRepository(_context, _appsettings);
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
