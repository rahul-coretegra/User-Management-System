using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.PostgreSqlModels
{
    
    public class ConfigureService
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public string UniqueId { get; set; }

        [Required]
        public string ServiceUniqueId { get; set; }

        [Required]
        public string ServiceName { get; set; }

        [Required]
        public TypeOfService ServiceType { get; set; }

        public List<Item> Items { get; set; }

    }
}
