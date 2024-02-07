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
            UserAndRoles = new UserAndRolesRepository(_context);
            RoleAndAccess = new RoleAndAccessRepository(_context);

        }
        public IUserRoleRepository UserRoles { private set; get; }

        public IUserRepository Users { private set; get; }

        public IRouteRepository Routes { private set; get; }

        public IUserAndRolesRepository UserAndRoles { private set; get; }

        public IRoleAndAccessRepository RoleAndAccess { private set; get; }
    }
}
