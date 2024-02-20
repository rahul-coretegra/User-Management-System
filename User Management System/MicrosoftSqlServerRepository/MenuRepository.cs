using Microsoft.EntityFrameworkCore;
using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MicrosoftSqlServerModels;
using User_Management_System.MicrosoftSqlServerRepository.IMSSqlServerRepository;

namespace User_Management_System.MicrosoftSqlServerRepository
{
    public class MenuRepository:Repository<Menu>,IMenuRepository
    {
        private readonly MicrosoftSqlServerApplicationDbContext _context;

        public MenuRepository(MicrosoftSqlServerApplicationDbContext options) : base(options)
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
