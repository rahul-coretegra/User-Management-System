using User_Management_System.ManagementConfigurations;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementRepository.IManagementRepository;

namespace User_Management_System.ManagementRepository
{
    public class ProjectRepository : Repository<Project>, IProjectReporistory
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
