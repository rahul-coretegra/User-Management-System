using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.ManagementModels.VMs
{
    public class RoleAndAccessVM
    {
        public string UniqueId { get; set; }

        [Required]
        public string RoleId { get; set; }

        [Required]
        public string RouteId { get; set; }

        public TrueFalse IsAccess { get; set; }

    }
}
