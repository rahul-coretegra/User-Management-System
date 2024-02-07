using Microsoft.EntityFrameworkCore;
using User_Management_System.ManagementConfigurations;

using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MicrosoftSqlServerModels;
using User_Management_System.MicrosoftSqlServerRepository.IMSSqlServerRepository;

namespace User_Management_System.MicrosoftSqlServerRepository
{
    public class UserAndRolesRepository: Repository<UserAndRoles>, IUserAndRolesRepository
    {
        private readonly MicrosoftSqlServerApplicationDbContext _context;

        public UserAndRolesRepository(MicrosoftSqlServerApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
