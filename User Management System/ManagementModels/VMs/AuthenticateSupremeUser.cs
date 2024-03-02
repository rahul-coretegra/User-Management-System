using System.ComponentModel.DataAnnotations;

namespace User_Management_System.ManagementModels.VMs
{
    public class AuthenticateSupremeUser
    {
        [Required]
        public string Identity { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
