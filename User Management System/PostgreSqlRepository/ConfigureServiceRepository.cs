using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlModels;
using User_Management_System.PostgreSqlRepository.IPsqlRepository;

namespace User_Management_System.PostgreSqlRepository
{
    public class ConfigureServiceRepository : Repository<ConfigureService>, IConfigureServiceRepository
    {
        private readonly PostgreSqlApplicationDbContext _context;
        public ConfigureServiceRepository(PostgreSqlApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
