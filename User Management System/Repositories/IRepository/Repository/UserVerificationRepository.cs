using Microsoft.EntityFrameworkCore;
using User_Management_System.DbModule;
using User_Management_System.Models.SupremeModels;
using User_Management_System.Models.UserModels;

namespace User_Management_System.Repositories.IRepository.Repository
{
    public class UserVerificationRepository : Repository<UserVerification>, IUserVerificationRepository
    {
        private readonly ApplicationDbContext _context;

        public UserVerificationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public bool IsOtpExpired(UserVerification user)
        {
            if (user.otpTimeStamp.HasValue)
            {
                DateTime expirationTime = user.otpTimeStamp.Value.Add(TimeSpan.FromMinutes(3));
                if (expirationTime < DateTime.UtcNow)
                {
                    user.otp = null;
                    user.otpTimeStamp = null;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> IsVerified(string Identity, string Otp)
        {
            var indb = await _context.UserVerifications.FirstOrDefaultAsync(x => x.identity == Identity);

            if (indb.otp == Otp)
                return true;

            return false;
        }
    }
}
