using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.ManagementModels.VMs
{
    public class MenuAccessVM
    {
        public string UniqueId { get; set; }

        [Required]
        public string RoleId { get; set; }

        [Required]
        public string MenuId { get; set; }

        [Required]
        public TrueFalse IsAccess { get; set; }
    }


    public class MongoMenuAccess
    {
        public string Id { get; set; }

        public string UniqueId { get; set; }

        public string RoleId { get; set; }

        public MongoDbModels.UserRole UserRole { get; set; }

        public string MenuId { get; set; }

        public MongoDbModels.Menu Menu { get; set; }

        public TrueFalse IsAccess { get; set; }

    }
}
