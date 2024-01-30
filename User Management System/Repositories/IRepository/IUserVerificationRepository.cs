using User_Management_System.Models.UserModels;

namespace User_Management_System.Repositories.IRepository
{
    public interface IUserVerificationRepository:IRepository<UserVerification>
    {
        public bool IsOtpExpired(UserVerification user);

        public Task<bool> IsVerified(string Identity, string Otp);

    }
}
