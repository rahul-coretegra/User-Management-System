using Microsoft.EntityFrameworkCore;
using User_Management_System.ManagementConfigurations;

using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlModels;
using User_Management_System.PostgreSqlRepository.IPsqlRepository;

namespace User_Management_System.PostgreSqlRepository
{
    public class UserAndRolesRepository: Repository<UserAndRoles>, IUserAndRolesRepository
    {
        private readonly PostgreSqlApplicationDbContext _context;

        public UserAndRolesRepository(PostgreSqlApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
