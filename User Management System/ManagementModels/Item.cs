using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace User_Management_System.ManagementModels
{
    public class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        [Key]
        public string ItemUniqueId { get; set; }

        [Required]
        public string ItemName { get; set; }

        public string ServiceUniqueId { get; set; }

        public Service Service { get; set; }
    }
}

