using Microsoft.EntityFrameworkCore;
using User_Management_System.MongoDbConfigurations;
using User_Management_System.MongoDbModels;
using User_Management_System.MongoDbRepository.IMongoRepository;

namespace User_Management_System.MongoDbRepository
{
    public class MenuAccessRepository:Repository<MenuAccess>,IMenuAccessRepository
    {
        private readonly MongoDbApplicationDbContext _context;

        public MenuAccessRepository(MongoDbApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}
