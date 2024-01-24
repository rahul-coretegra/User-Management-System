using User_Management_System.DbModule;

namespace User_Management_System.Repositories.IRepository.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            UserRoles = new UserRoleRepository(_context);
            Users = new UserRepository(_context);

        }
        public IUserRoleRepository UserRoles { private set; get; }

        public IUserRepository Users { private set; get; }


        public string GenrateAlphaNumricUniqueCode()
        {
            DateTime now = DateTime.Now;
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            string additionalDigits = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());

            string resultString = $"{additionalDigits}{now:yyyymmddHHssffff}";

            return resultString;
        }
    }
}
