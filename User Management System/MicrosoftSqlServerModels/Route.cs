using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace User_Management_System.MicrosoftSqlServerModels
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
    }
}
