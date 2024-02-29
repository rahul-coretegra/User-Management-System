using System.ComponentModel.DataAnnotations;

namespace User_Management_System.ManagementModels.VMs
{
    public class ConfigureItem
    {
        [Required]
        public string ItemId { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string ItemValue { get; set; }

    }
}
