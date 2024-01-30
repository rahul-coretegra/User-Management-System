
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User_Management_System.DbModule;
using User_Management_System.Models.EnumModels;
using User_Management_System.Models.SupremeModels;
using User_Management_System.Models.UserModels;

namespace User_Management_System.Repositories.RegisterAndAuthenticate
{
    public class RegisterAndAuthenticationRepository : IRegisterAndAuthenticateRepository
    {

        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _context;

        public RegisterAndAuthenticationRepository(ApplicationDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<bool> IsUniqueUser(string Phonenumber, string Email)
        {
            var userindb = await _context.Users.FirstOrDefaultAsync(x => x.phoneNumber == Phonenumber || x.email == Email);
            if (userindb == null)
                return true;
            return false;
        }

        public async Task<string> Authenticate(string Identity, string RoleUniqueCode)
        {
            var userindb = await _context.Users.FirstOrDefaultAsync(x => x.phoneNumber == Identity || x.userUniqueCode == Identity || x.email == Identity);
            if (userindb == null)
                return null;
            //jwt
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescritor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, RoleUniqueCode),
                    new Claim(ClaimTypes.MobilePhone, userindb.phoneNumber),
                    new Claim(ClaimTypes.Email, userindb.email)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescritor);
            userindb.token = tokenHandler.WriteToken(token);
            return userindb.token;
        }

        public async Task<bool> RegisterUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}






