using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User_Management_System.ManagementConfigurations;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementRepository.IManagementRepository;

namespace User_Management_System.ManagementRepository
{
    public class SupremeUserRepository : Repository<SupremeUser>, ISupremeUserRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly AppSettings _settings;

        public SupremeUserRepository(ApplicationDbContext context, IOptions<AppSettings> settings) : base(context)
        {
            _context = context;
            _settings = settings.Value;
        }

        public async Task<bool> IsUniqueUser(string Phonenumber, string Email)
        {
            var userindb = await _context.SupremeUsers.FirstOrDefaultAsync(x => x.PhoneNumber == Phonenumber || x.Email == Email);
            if (userindb == null)
                return true;
            return false;
        }

        public async Task<string> Authenticate(string Identity)
        {
            var userindb = await _context.SupremeUsers.FirstOrDefaultAsync(x => x.PhoneNumber == Identity || x.UniqueId == Identity || x.Email == Identity);
            if (userindb == null)
                return null;
            //jwt
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescritor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.SerialNumber, userindb.UniqueId),
                    new Claim(ClaimTypes.MobilePhone, userindb.PhoneNumber),
                    new Claim(ClaimTypes.Email, userindb.Email),
                    new Claim("SupremeAcces", userindb.SupremeAccess? "true" : "false")

                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescritor);
            userindb.Token = tokenHandler.WriteToken(token);
            return userindb.Token;
        }

        public async Task<bool> RegisterUser(SupremeUser user)
        {
            await _context.SupremeUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}