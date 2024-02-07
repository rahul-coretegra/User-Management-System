using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.PostgreSqlModels
{
    public class UserAndRoles
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public string UniqueId { get; set; }

        [Required]
        public string UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [Required]
        public string RoleId { get; set; }

        public UserRole UserRole { get; set; }

        public TrueFalse AccessToRole { get; set; }

    }
}
