

namespace User_Management_System.ManagementRepository.IManagementRepository
{
    public interface IManagementWork
    {
        IProjectReporistory Projects { get; }

        ISupremeUserRepository SupremeUsers { get; }

        string UniqueId();
    }
}
