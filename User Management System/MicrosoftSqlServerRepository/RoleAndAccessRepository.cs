using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MicrosoftSqlServerModels;
using User_Management_System.MicrosoftSqlServerRepository.IMSSqlServerRepository;

namespace User_Management_System.MicrosoftSqlServerRepository
{
    public class RoleAndAccessRepository : Repository<RoleAndAccess>, IRoleAndAccessRepository
    {
        private readonly MicrosoftSqlServerApplicationDbContext _context;

        public RoleAndAccessRepository(MicrosoftSqlServerApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
