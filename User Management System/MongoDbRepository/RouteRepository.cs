using User_Management_System.MongoDbConfigurations;
using User_Management_System.MongoDbRepository.IMongoRepository;

namespace User_Management_System.MongoDbRepository
{
    public class RouteRepository:Repository<MongoDbModels.Route>, IRouteRepository
    {
        private readonly MongoDbApplicationDbContext _context;
        public RouteRepository(MongoDbApplicationDbContext dbContext): base(dbContext) 
        {
            _context = dbContext;  
        }
    }
}
