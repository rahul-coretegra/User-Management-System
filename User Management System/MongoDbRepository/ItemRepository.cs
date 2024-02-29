using User_Management_System.MongoDbConfigurations;
using User_Management_System.MongoDbModels;
using User_Management_System.MongoDbRepository.IMongoRepository;

namespace User_Management_System.MongoDbRepository
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        private readonly MongoDbApplicationDbContext _context;

        public ItemRepository(MongoDbApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
