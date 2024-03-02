using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.PostgreSqlModels
{
    public class RouteAccess
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public string UniqueId { get; set; }

        [Required]
        public string RoleId { get; set; }

        public UserRole UserRole { get; set; }

        [Required]
        public string RouteId { get; set; }

        public Route Route { get; set; }

        [Required]
        public TrueFalse IsAccess { get; set; }

    }
}
