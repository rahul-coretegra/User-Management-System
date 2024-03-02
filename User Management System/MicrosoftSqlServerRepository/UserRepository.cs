using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MicrosoftSqlServerModels;
using User_Management_System.MicrosoftSqlServerRepository.IMSSqlServerRepository;

namespace User_Management_System.MicrosoftSqlServerRepository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly MicrosoftSqlServerApplicationDbContext _context;
        private readonly AppSettings _settings;


        public UserRepository(MicrosoftSqlServerApplicationDbContext options, IOptions<AppSettings> settings) : base(options)
        {
            _context = options;
            _settings = settings.Value;
        }
        public async Task<bool> IsUniqueUser(string Phonenumber, string Email)
        {
            try
            {
                var userindb = await dbSet.FirstOrDefaultAsync(x => x.PhoneNumber == Phonenumber || x.Email == Email);
                if (userindb == null)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        public async Task<string> Authenticate(string Identity, string RoleId)
        {
            try
            {
                var userindb = await dbSet.FirstOrDefaultAsync(x => x.PhoneNumber == Identity || x.UserId == Identity || x.Email == Identity);
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
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        public async Task<bool> RegisterUser(User user)
        {
            try
            {
                await dbSet.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
    }
}
