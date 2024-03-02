using Microsoft.EntityFrameworkCore;
using User_Management_System.ManagementConfigurations;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementRepository.IManagementRepository;

namespace User_Management_System.ManagementRepository
{
    public class ConfigureServiceRepository : Repository<ConfigureService>, IConfigureServiceRepository
    {
        private readonly ApplicationDbContext _context;
        public ConfigureServiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
