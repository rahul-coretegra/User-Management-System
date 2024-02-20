using User_Management_System.ManagementModels;

namespace User_Management_System.ManagementRepository.IManagementRepository
{
    public interface ISupremeUserRepository:IRepository<SupremeUser>
    {
        public Task<bool> IsUniqueUser(string Phonenumber, string Email);

        public Task<bool> RegisterUser(SupremeUser User);

        public Task<string> Authenticate(string Identity);
    }
}
