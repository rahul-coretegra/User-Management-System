using Microsoft.EntityFrameworkCore;
using User_Management_System.MongoDbConfigurations;
using User_Management_System.MongoDbModels;
using User_Management_System.MongoDbRepository.IMongoRepository;

namespace User_Management_System.MongoDbRepository
{
    public class UserAndRolesRepository : Repository<UserAndRoles>, IUserAndRolesRepository
    {
        private readonly MongoDbApplicationDbContext _context;
        public UserAndRolesRepository(MongoDbApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }

}
