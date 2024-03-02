
namespace User_Management_System.PostgreSqlRepository.IPsqlRepository
{
    public interface IPsqlUnitOfWork
    {
        IUserRoleRepository UserRoles { get; }

        IUserRepository Users { get; }

        IRouteRepository Routes { get; }

        IRoleAccessRepository RoleAccess { get; }

        IRouteAccessRepository RouteAccess { get; }

        IMenuRepository Menus{ get; } 

        IMenuAccessRepository MenuAccess { get; }

        IServiceRepository Services { get; }

        IConfigureServiceRepository ConfigureServices { get; }


    }
}
