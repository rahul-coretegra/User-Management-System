using User_Management_System.MongoDbModels;

namespace User_Management_System.MongoDbRepository.IMongoRepository
{
    public interface IMenuRepository:IRepository<Menu>
    {
        public object GetMenuWithSubmenus(Menu Menu, List<Menu> AllMenus);
    }
}
