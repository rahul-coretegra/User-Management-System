using Microsoft.EntityFrameworkCore;
using User_Management_System.MongoDbConfigurations;
using User_Management_System.MongoDbModels;
using User_Management_System.MongoDbRepository.IMongoRepository;

namespace User_Management_System.MongoDbRepository
{
    public class RoleAndMenusRepository:Repository<RoleAndMenus>,IRoleAndMenusRepository
    {
        private readonly MongoDbApplicationDbContext _context;

        public RoleAndMenusRepository(MongoDbApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}
