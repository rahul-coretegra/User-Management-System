using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.ManagementModels.VMs
{
    public class ServiceVM
    {
        public string ServiceId { get; set; }

        [Required]
        public string ServiceUniqueId { get; set; }

        [Required]
        public string ServiceName { get; set; }

        [Required]
        public TypeOfService ServiceType { get; set; }

        [Required]
        public TrueFalse Status { get; set; }
    }
}
