namespace User_Management_System.MicrosoftSqlServerRepository.IMSSqlServerRepository
{
    public interface IMsSqlUnitOfWork
    {
        IUserRoleRepository UserRoles { get; }

        IUserRepository Users { get; }

        IRouteRepository Routes { get; }

        IRoleAccessRepository RoleAccess { get; }

        IRouteAccessRepository RouteAccess { get; }

        IMenuRepository Menus { get; }

        IMenuAccessRepository MenuAccess { get; }

        IServiceRepository Services { get; }

        IConfigureServiceRepository ConfigureServices { get; }


    }
}
