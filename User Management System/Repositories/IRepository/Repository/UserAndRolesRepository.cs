using User_Management_System.DbModule;
using User_Management_System.Models.SupremeModels;

namespace User_Management_System.Repositories.IRepository.Repository
{
    public class UserAndRolesRepository: Repository<UserAndRoles>, IUserAndRolesRepository
    {
        private readonly ApplicationDbContext _context;

        public UserAndRolesRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
