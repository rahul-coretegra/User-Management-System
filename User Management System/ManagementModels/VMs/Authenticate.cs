using System.ComponentModel.DataAnnotations;

namespace User_Management_System.ManagementModels.VMs
{
    public class Authenticate
    {
        [Required]
        public string Identity { get; set; }
        [Required]
        public string Password { get; set; }

        public string RoleId { get; set; }
    }
}
