using User_Management_System.DbModule;
using User_Management_System.Models.SupremeModels;

namespace User_Management_System.Repositories.IRepository.Repository
{
    public class RoleAndAccessRepository : Repository<RoleAndAccess>, IRoleAndAccessRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleAndAccessRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
