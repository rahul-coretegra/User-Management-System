
using System.ComponentModel.DataAnnotations;

namespace User_Management_System.Models.UserModels.UserVM
{
    public class UpdateUserVM
    {
        [Required]
        public string userUniqueCode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression("^([a-zA-z\\s]{3,50})$")]
        public string username { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression("^[789]\\d{9}$")]
        public string phoneNumber { get; set; }

        [Required]
        [RegularExpression("^[a-z0-9+_.-]+@[a-z0-9.-]+$")]
        public string email { get; set; }

        [Required]
        public string address { get; set; }
    }
}
