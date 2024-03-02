using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.PostgreSqlModels
{
    public class Menu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public string MenuId { get; set; }

        [Required]
        public string MenuName { get; set; }

        [Required]
        public string MenuPath { get; set; }

        public string MenuIcon { get; set; }

        public string ParentId { get; set; }

        [Required]
        public TrueFalse Status { get; set; }
    }
}
