using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MicrosoftSqlServerModels;
using User_Management_System.MicrosoftSqlServerRepository.IMSSqlServerRepository;

namespace User_Management_System.MicrosoftSqlServerRepository
{
    public class RouteAccessRepository : Repository<RouteAccess>, IRouteAccessRepository
    {
        private readonly MicrosoftSqlServerApplicationDbContext _context;

        public RouteAccessRepository(MicrosoftSqlServerApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
