using Microsoft.EntityFrameworkCore;
using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlModels;
using User_Management_System.PostgreSqlRepository.IPsqlRepository;

namespace User_Management_System.PostgreSqlRepository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly PostgreSqlApplicationDbContext _context;

        public UserRepository(PostgreSqlApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
