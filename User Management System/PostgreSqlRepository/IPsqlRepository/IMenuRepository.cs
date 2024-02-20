using User_Management_System.PostgreSqlModels;

namespace User_Management_System.PostgreSqlRepository.IPsqlRepository
{
    public interface IMenuRepository:IRepository<Menu>
    {
        public object GetMenuWithSubmenus(Menu Menu, List<Menu> AllMenus);
    }
}
