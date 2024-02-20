using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlModels;
using User_Management_System.PostgreSqlRepository.IPsqlRepository;

namespace User_Management_System.PostgreSqlRepository
{
    public class MenuRepository:Repository<Menu>, IMenuRepository
    {
        private readonly PostgreSqlApplicationDbContext _context;
        public MenuRepository(PostgreSqlApplicationDbContext options) : base(options)
        {
            _context = options;
        }

        public object GetMenuWithSubmenus(Menu Menu, List<Menu> AllMenus)
        {
            return new
            {
                id = Menu.Id,
                UniqueId = Menu.MenuId,
                MenuName = Menu.MenuName,
                MenuPath = Menu.MenuPath,
                MenuIcon = Menu.MenuIcon,
                Status = Menu.Status,
                ParentId = Menu.ParentId,
                SubMenus = AllMenus
                    .Where(subMenu => subMenu.ParentId == Menu.MenuId)
                    .Select(subMenu => GetMenuWithSubmenus(subMenu, AllMenus))
                    .ToList()
            };
        }
    }
}
