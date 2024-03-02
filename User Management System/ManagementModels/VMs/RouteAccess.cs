using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.ManagementModels.VMs
{
    public class RouteAccess
    {
        public string UniqueId { get; set; }

        [Required]
        public string RoleId { get; set; }

        [Required]
        public string RouteId { get; set; }

        [Required]
        public TrueFalse IsAccess { get; set; }

    }

    public class MongoRouteAccess
    {
        public string Id { get; set; }

        public string UniqueId { get; set; }

        public string RoleId { get; set; }

        public MongoDbModels.UserRole UserRole { get; set; }

        public string RouteId { get; set; }

        public MongoDbModels.Route Route { get; set; }

        public TrueFalse IsAccess { get; set; }

    }
}
