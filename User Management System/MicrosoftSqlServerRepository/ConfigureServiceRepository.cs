using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MicrosoftSqlServerModels;
using User_Management_System.MicrosoftSqlServerRepository.IMSSqlServerRepository;

namespace User_Management_System.MicrosoftSqlServerRepository
{
    public class ConfigureServiceRepository : Repository<ConfigureService>, IConfigureServiceRepository
    {
        private readonly MicrosoftSqlServerApplicationDbContext _context;
        public ConfigureServiceRepository(MicrosoftSqlServerApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
