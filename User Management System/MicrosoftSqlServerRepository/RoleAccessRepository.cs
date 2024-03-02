using Microsoft.EntityFrameworkCore;
using User_Management_System.ManagementConfigurations;

using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MicrosoftSqlServerModels;
using User_Management_System.MicrosoftSqlServerRepository.IMSSqlServerRepository;

namespace User_Management_System.MicrosoftSqlServerRepository
{
    public class RoleAccessRepository: Repository<RoleAccess>, IRoleAccessRepository
    {
        private readonly MicrosoftSqlServerApplicationDbContext _context;

        public RoleAccessRepository(MicrosoftSqlServerApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
