using Microsoft.EntityFrameworkCore;
using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlModels;
using User_Management_System.PostgreSqlRepository.IPsqlRepository;

namespace User_Management_System.PostgreSqlRepository
{
    public class RouteAccessRepository : Repository<RouteAccess>, IRouteAccessRepository
    {
        private readonly PostgreSqlApplicationDbContext _context;

        public RouteAccessRepository(PostgreSqlApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
