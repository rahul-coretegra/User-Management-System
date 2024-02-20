using Microsoft.EntityFrameworkCore;
using User_Management_System.MongoDbConfigurations;
using User_Management_System.MongoDbModels;
using User_Management_System.MongoDbRepository.IMongoRepository;

namespace User_Management_System.MongoDbRepository
{
    public class MenuRepository:Repository<Menu>, IMenuRepository
    {
        private readonly MongoDbApplicationDbContext _context;

        public MenuRepository(MongoDbApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
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
