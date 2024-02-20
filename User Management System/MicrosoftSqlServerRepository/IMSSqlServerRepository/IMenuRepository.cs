using User_Management_System.MicrosoftSqlServerModels;

namespace User_Management_System.MicrosoftSqlServerRepository.IMSSqlServerRepository
{
    public interface IMenuRepository:IRepository<Menu>
    {
        public object GetMenuWithSubmenus(Menu Menu, List<Menu> AllMenus);
    }
}
