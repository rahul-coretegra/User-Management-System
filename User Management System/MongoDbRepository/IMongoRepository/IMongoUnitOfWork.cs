namespace User_Management_System.MongoDbRepository.IMongoRepository
{
    public interface IMongoUnitOfWork
    {
        IUserRoleRepository UserRoles { get; }

        IUserRepository Users { get; }

        IRouteRepository Routes { get; }

        IUserAndRolesRepository UserAndRoles { get; }

        IRoleAndAccessRepository RoleAndAccess { get; }

        IMenuRepository Menus { get; }

        IRoleAndMenusRepository RoleAndMenus { get; }

        IItemRepository Items { get; }

        IConfigureServiceRepository ConfigureServices { get; }

    }
}
