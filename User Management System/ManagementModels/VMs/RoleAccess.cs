using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.ManagementModels.VMs
{
    public class RoleAccess
    {
        public string UniqueId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleId { get; set; }

        public TrueFalse AccessToRole { get; set; }
    }
}
