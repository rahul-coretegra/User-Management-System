using Microsoft.EntityFrameworkCore;
using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlRepository.IPsqlRepository;

namespace User_Management_System.PostgreSqlRepository
{
    public class PsqlUnitOfWork : IPsqlUnitOfWork
    {
        private readonly PostgreSqlApplicationDbContext _context;

        public PsqlUnitOfWork(PostgreSqlApplicationDbContext context)
        {
            _context = context;
            UserRoles = new UserRoleRepository(_context);
            Users = new UserRepository(_context);
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
