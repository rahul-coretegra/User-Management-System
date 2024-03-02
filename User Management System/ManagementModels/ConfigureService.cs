using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.ManagementModels
{
    public class ConfigureService
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public string UniqueId { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string ServiceUniqueId { get; set; }

        public Service Service { get; set; }

        [Required]
        public TrueFalse IsConfigured { get; set; }

    }
}
