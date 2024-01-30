using User_Management_System.Models.SupremeModels;
using User_Management_System.Models.UserModels;

namespace User_Management_System.Repositories.RegisterAndAuthenticate
{
    public interface IRegisterAndAuthenticateRepository
    {
        public Task<bool> IsUniqueUser(string Phonenumber , string Email);

        public Task<bool> RegisterUser(User User);

        public Task<string> Authenticate(string Identity, string RoleUniqueCode);
    }
}
