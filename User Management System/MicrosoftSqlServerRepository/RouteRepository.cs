
using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MicrosoftSqlServerRepository.IMSSqlServerRepository;

namespace User_Management_System.MicrosoftSqlServerRepository
{
    public class RouteRepository : Repository<MicrosoftSqlServerModels.Route>, IRouteRepository
    {
        private readonly MicrosoftSqlServerApplicationDbContext _context;

        public RouteRepository(MicrosoftSqlServerApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
