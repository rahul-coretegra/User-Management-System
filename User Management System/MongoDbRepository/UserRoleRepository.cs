using User_Management_System.MongoDbConfigurations;
using User_Management_System.MongoDbModels;
using User_Management_System.MongoDbRepository.IMongoRepository;

namespace User_Management_System.MongoDbRepository
{
    public class UserRoleRepository:Repository<UserRole>, IUserRoleRepository
    {
        private readonly MongoDbApplicationDbContext context;
        public UserRoleRepository(MongoDbApplicationDbContext dbContext):base(dbContext) 
        {
            context = dbContext; 
        }
    }
}
