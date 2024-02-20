using System.Linq.Expressions;
using User_Management_System.MongoDbModels;

namespace User_Management_System.MongoDbRepository.IMongoRepository
{
    public interface IUserRepository:IRepository<User>
    {
        public Task<bool> IsUniqueUser(string Phonenumber, string Email);

        public Task<bool> RegisterUser(User User);

        public Task<string> Authenticate(string Identity, string RoleId);
    }
}
