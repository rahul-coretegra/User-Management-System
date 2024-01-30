using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace User_Management_System.Models.UserModels
{
    public class UserVerification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Key]
        public string identity { get; set; }

        public string otp { get; set; }

        public DateTime? otpTimeStamp { get; set; }
    }
}
