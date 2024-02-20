using Microsoft.EntityFrameworkCore;
using User_Management_System.MongoDbConfigurations;
using User_Management_System.MongoDbModels;
using User_Management_System.MongoDbRepository.IMongoRepository;

namespace User_Management_System.MongoDbRepository
{
    public class RoleAndAccessRepository:Repository<RoleAndAccess>, IRoleAndAccessRepository
    {
        private readonly MongoDbApplicationDbContext _context;

        public RoleAndAccessRepository(MongoDbApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}
