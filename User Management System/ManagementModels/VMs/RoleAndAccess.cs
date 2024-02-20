using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.ManagementModels.VMs
{
    public class RoleAndAccess
    {
        public string UniqueId { get; set; }

        [Required]
        public string RoleId { get; set; }

        [Required]
        public string RouteId { get; set; }

        public TrueFalse IsAccess { get; set; }

    }

    public class MongoRoleAndAccess
    {
        public string Id { get; set; }

        public string UniqueId { get; set; }

        [Required]
        public string RoleId { get; set; }

        public MongoDbModels.UserRole UserRole { get; set; }

        [Required]
        public string RouteId { get; set; }

        public MongoDbModels.Route Route { get; set; }

        public TrueFalse IsAccess { get; set; }

    }
}
