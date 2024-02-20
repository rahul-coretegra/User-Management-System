
using Microsoft.EntityFrameworkCore;
using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MicrosoftSqlServerModels;
using User_Management_System.MicrosoftSqlServerRepository.IMSSqlServerRepository;

namespace User_Management_System.MicrosoftSqlServerRepository
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        private readonly MicrosoftSqlServerApplicationDbContext _context;

        public UserRoleRepository(MicrosoftSqlServerApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
