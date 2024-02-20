using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.MongoDbModels
{
    public class UserRole
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string RoleId { get; set; }

        [BsonRequired]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression("^(?=.*[a-zA-Z])[a-zA-Z ]{3,50}$")]
        public string RoleName { get; set; }

        [BsonRequired]
        public RoleLevels RoleLevel { get; set; }
    }
}
