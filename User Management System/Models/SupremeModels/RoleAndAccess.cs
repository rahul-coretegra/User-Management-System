using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using User_Management_System.Models.EnumModels;

namespace User_Management_System.Models.SupremeModels
{
    public class RoleAndAccess
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Key]
        public string roleAndAccessId { get; set; }

        [Required]
        public string routePath { get; set; }

        [Required]
        public string routeName { get; set; }

        [Required]
        public string roleUniqueCode { get; set; }

        public UserRole userRole { get; set; }

        public TrueFalse isAccess { get; set; }

    }
}
