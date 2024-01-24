
using User_Management_System.DbModule;
using User_Management_System.Models.SupremeModels;

namespace User_Management_System.Repositories.IRepository.Repository
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
