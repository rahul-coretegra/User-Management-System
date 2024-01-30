
namespace User_Management_System.Repositories.IRepository
{
    public interface IUnitOfWork
    {
        IUserRoleRepository UserRoles { get; }

        IUserRepository Users { get; }

        IUserAndRolesRepository UserAndRoles { get; }

        IUserVerificationRepository UserVerifications { get; }

        IRoleAndAccessRepository RoleAndAccess { get; }

        string GenrateAlphaNumricUniqueCode();
    }
}
