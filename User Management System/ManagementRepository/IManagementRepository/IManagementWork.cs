

namespace User_Management_System.ManagementRepository.IManagementRepository
{
    public interface IManagementWork
    {
        IProjectReporistory Projects { get; }

        ISupremeUserRepository SupremeUsers { get; }

        IServiceRepository Services { get; }

        IConfigureServiceRepository ConfigureServices { get; }

        string UniqueId();
    }
}
