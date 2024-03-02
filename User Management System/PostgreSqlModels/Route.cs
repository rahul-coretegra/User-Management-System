using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.PostgreSqlModels
{
    public class Route
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public string RouteId { get; set; }

        [Required]
        public string RoutePath { get; set; }

        [Required]
        public string RouteName { get; set; }

        [Required]
        public TrueFalse Status { get; set; }

    }
}
