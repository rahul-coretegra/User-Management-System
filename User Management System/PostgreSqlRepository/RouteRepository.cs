using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlRepository.IPsqlRepository;

namespace User_Management_System.PostgreSqlRepository
{
    public class RouteRepository : Repository<PostgreSqlModels.Route>, IRouteRepository
    {
        private readonly PostgreSqlApplicationDbContext _context;

        public RouteRepository(PostgreSqlApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
