using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.ManagementModels.VMs
{
    public class Menu
    {
        public string Id { get; set; }

        public string MenuId { get; set; }

        [Required]
        public string MenuName { get; set; }

        [Required]
        public string MenuPath { get; set; }

        public string MenuIcon { get; set; }

        public TrueFalse Status { get; set; }

        public string ParentId { get; set; }
    }
}
