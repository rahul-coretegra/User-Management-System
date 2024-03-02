using Microsoft.EntityFrameworkCore;
using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlModels;
using User_Management_System.PostgreSqlRepository.IPsqlRepository;

namespace User_Management_System.PostgreSqlRepository
{
    public class MenuAccessRepository:Repository<MenuAccess>, IMenuAccessRepository
    {
        private readonly PostgreSqlApplicationDbContext _context;
        public MenuAccessRepository(PostgreSqlApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
