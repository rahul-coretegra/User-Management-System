using System.ComponentModel.DataAnnotations;
using User_Management_System.Models.EnumModels;
using User_Management_System.Models.SupremeModels;

namespace User_Management_System.Models.UserModels.UserViewModels
{
    public class UserAndRoleVM
    {
        public string userUniqueCode { get; set; }

        public string username { get; set; }

        public string phoneNumber { get; set; }

        public string email { get; set; }

        public string address { get; set; }

        public UserRole userRole { get; set; }

        public TrueFalse accessToRole { get; set; }

    }
}
