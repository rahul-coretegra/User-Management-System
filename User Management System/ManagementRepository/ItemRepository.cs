using User_Management_System.ManagementConfigurations;
using User_Management_System.ManagementModels;
using User_Management_System.ManagementRepository.IManagementRepository;

namespace User_Management_System.ManagementRepository
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        private readonly ApplicationDbContext _context;
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
