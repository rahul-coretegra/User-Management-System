
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User_Management_System.ManagementConfigurations;
using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlModels;

namespace User_Management_System.PostgreSqlRepository.RegisterAndAuthenticate
{
    public class RegisterAndAuthenticationRepository : IRegisterAndAuthenticateRepository
    {
        private readonly PostgreSqlApplicationDbContext _psqlcontext;
        private readonly AppSettings _settings;

        public RegisterAndAuthenticationRepository(PostgreSqlApplicationDbContext context, IOptions<AppSettings> settings)          
        {
            _psqlcontext = context;
            _settings = settings.Value;

        }

        public async Task<bool> IsUniqueUser(string Phonenumber, string Email)
        {
            var userindb = await _psqlcontext.Users.FirstOrDefaultAsync(x => x.PhoneNumber == Phonenumber || x.Email == Email);
            if (userindb == null)
                return true;
            return false;
        }

        public async Task<string> Authenticate(string Identity, string RoleId)
        {
            
            var userindb = await _psqlcontext.Users.FirstOrDefaultAsync(x => x.PhoneNumber == Identity || x.UserId == Identity || x.Email == Identity);
            if (userindb == null)
                return null;
            //jwt
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescritor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.SerialNumber, userindb.UserId),
                    new Claim(ClaimTypes.MobilePhone, userindb.PhoneNumber),
                    new Claim(ClaimTypes.Email, userindb.Email),
                    new Claim(ClaimTypes.Role, RoleId)

                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescritor);
            userindb.Token = tokenHandler.WriteToken(token);
            return userindb.Token;
        }

        public async Task<bool> RegisterUser(User user)
        {
            await _psqlcontext.Users.AddAsync(user);
            await _psqlcontext.SaveChangesAsync();
            return true;
        }
    }
}






