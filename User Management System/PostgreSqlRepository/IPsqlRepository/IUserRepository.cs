using User_Management_System.PostgreSqlModels;

namespace User_Management_System.PostgreSqlRepository.IPsqlRepository
{
    public interface IUserRepository:IRepository<User>
    {
        public Task<bool> IsUniqueUser(string Phonenumber, string Email);

        public Task<bool> RegisterUser(User User);

        public Task<string> Authenticate(string Identity, string RoleId);
    }
}
