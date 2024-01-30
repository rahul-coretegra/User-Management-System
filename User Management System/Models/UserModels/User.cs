using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using User_Management_System.Models.SupremeModels;
using User_Management_System.Models.EnumModels;

namespace User_Management_System.Models.UserModels
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Key]
        public string userUniqueCode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression("^([a-zA-z\\s]{3,50})$")]
        public string username { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression("^[789]\\d{9}$")]
        public string phoneNumber { get; set; }

        public List<UserAndRoles> userRoles { get; set; }

        [Required]
        [RegularExpression("^[a-z0-9+_.-]+@[a-z0-9.-]+$")]
        public string email { get; set; }

        [Required]
        public string address { get; set; }

        [Required]
        public string password { get; set; }

        public TrueFalse isVerifiedEmail { get; set; }

        public TrueFalse isVerifiedPhoneNumber { get; set; }

        public string token { get; set; }

        public TrueFalse isActiveUser { get; set; }

        public DateTime? createdAt { get; set; }

        public DateTime? updatedAt { get; set; }


    }
}
