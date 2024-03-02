using User_Management_System.MongoDbConfigurations;
using User_Management_System.MongoDbModels;
using User_Management_System.MongoDbRepository.IMongoRepository;

namespace User_Management_System.MongoDbRepository
{
    public class ConfigureServiceRepository : Repository<ConfigureService>, IConfigureServiceRepository
    {
        private readonly MongoDbApplicationDbContext _context;
        public ConfigureServiceRepository(MongoDbApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
