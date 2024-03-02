using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.MongoDbModels
{
    public class RoleAccess
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UniqueId { get; set; }

        [BsonRequired]
        public string UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [BsonRequired]
        public string RoleId { get; set; }

        public UserRole UserRole { get; set; }

        [BsonRequired]
        public TrueFalse AccessToRole { get; set; }
    }
}
