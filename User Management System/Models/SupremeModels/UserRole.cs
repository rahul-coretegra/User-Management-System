using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using User_Management_System.Models.EnumModels;
using User_Management_System.Models.UserModels;

namespace User_Management_System.Models.SupremeModels
{
    public class UserRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int roleId { get; set; }

        [Key]
        public string roleUniqueCode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression("^(?=.*[a-zA-Z])[a-zA-Z ]{3,50}$")]
        public string roleName { get; set; }

        [Required]
        public RoleLevels roleLevel { get; set; }
    }
}
