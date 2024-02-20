using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using User_Management_System.ManagementModels.EnumModels;
using User_Management_System.PostgreSqlModels;

namespace User_Management_System.ManagementModels.VMs
{
    public class UserAndRoles
    {
        public string UniqueId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleId { get; set; }

        public TrueFalse AccessToRole { get; set; }
    }
}
