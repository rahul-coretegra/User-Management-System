using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.ManagementModels
{
    public class Service
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceId { get; set; }

        [Key]
        public string ServiceUniqueId { get; set; }

        [Required]
        public string ServiceName { get; set; }

        [Required]
        public TypeOfService ServiceType { get; set; }

    }
}
