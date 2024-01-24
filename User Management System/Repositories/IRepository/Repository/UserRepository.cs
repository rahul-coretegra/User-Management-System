
using User_Management_System.DbModule;
using User_Management_System.Models.UserModels;

namespace User_Management_System.Repositories.IRepository.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
