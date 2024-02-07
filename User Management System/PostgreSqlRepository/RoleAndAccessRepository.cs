using Microsoft.EntityFrameworkCore;
using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlModels.SupremeModels;
using User_Management_System.PostgreSqlRepository.IPsqlRepository;

namespace User_Management_System.PostgreSqlRepository
{
    public class RoleAndAccessRepository : Repository<RoleAndAccess>, IRoleAndAccessRepository
    {
        private readonly PostgreSqlApplicationDbContext _context;

        public RoleAndAccessRepository(PostgreSqlApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
