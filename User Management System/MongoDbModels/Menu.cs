using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace User_Management_System.MongoDbModels
{
    public class Menu
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Key]
        public string MenuId { get; set; }

        [Required]
        public string MenuName { get; set; }

        [Required]
        public string MenuPath { get; set; }

        public string MenuIcon { get; set; }

        public TrueFalse Status { get; set; }

        public string ParentId { get; set; }
    }
}
