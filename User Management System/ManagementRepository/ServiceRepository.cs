using User_Management_System.ManagementConfigurations;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementRepository.IManagementRepository;

namespace User_Management_System.ManagementRepository
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _context;
        public ServiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
