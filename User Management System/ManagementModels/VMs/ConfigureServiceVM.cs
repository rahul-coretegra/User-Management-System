using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.ManagementModels.VMs
{
    public class ConfigureServiceVM
    {
        public string UniqueId { get; set; }

        [Required]
        public string ItemUniqueId { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string ItemValue { get; set; }

        [Required]
        public string ServiceId { get; set; }

        public Service Service { get; set; }

        [Required]
        public TrueFalse IsConfigured { get; set; }
    }
}
