
namespace User_Management_System.PostgreSqlRepository.IPsqlRepository
{
    public interface IPsqlUnitOfWork
    {
        IUserRoleRepository UserRoles { get; }

        IUserRepository Users { get; }

        IRouteRepository Routes { get; }

        IUserAndRolesRepository UserAndRoles { get; }

        IRoleAndAccessRepository RoleAndAccess { get; }

        IMenuRepository Menus{ get; } 

        IRoleAndMenusRepository RoleAndMenus { get; }

    }
}
