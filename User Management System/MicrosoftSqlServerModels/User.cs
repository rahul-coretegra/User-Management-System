using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.MicrosoftSqlServerModels
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public string UserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression("^([a-zA-z\\s]{3,50})$")]
        public string UserName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression("^[789]\\d{9}$")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^[a-z0-9+_.-]+@[a-z0-9.-]+$")]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Password { get; set; }

        public TrueFalse IsVerifiedEmail { get; set; }

        public TrueFalse IsVerifiedPhoneNumber { get; set; }

        public string Token { get; set; }

        public TrueFalse IsActiveUser { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

    }
}
