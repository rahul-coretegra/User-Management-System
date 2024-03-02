using User_Management_System.MongoDbConfigurations;
using User_Management_System.MongoDbModels;
using User_Management_System.MongoDbRepository.IMongoRepository;

namespace User_Management_System.MongoDbRepository
{
    public class ServiceRepository:Repository<Service>, IServiceRepository
    {
        private readonly MongoDbApplicationDbContext _context;

        public ServiceRepository(MongoDbApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
