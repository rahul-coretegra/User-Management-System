using System.ComponentModel.DataAnnotations;

namespace User_Management_System.ManagementModels.VMs
{
    public class RouteVM
    {
        public string RouteId { get; set; }

        [Required]
        public string RoutePath { get; set; }

        [Required]
        public string RouteName { get; set; }
    }
}
