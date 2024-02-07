using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.PostgreSqlRepository;
using User_Management_System.PostgreSqlRepository.RegisterAndAuthenticate;

namespace User_Management_System.ManagementModels
{
    public class Instance
    {
        public PostgreSqlApplicationDbContext PsqlDbContext {  get; set; }
        
        public RegisterAndAuthenticationRepository Psqlauthentication { get; set; }

        public PsqlUnitOfWork PsqlUOW { get; set; }
    }
}
