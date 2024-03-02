using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace User_Management_System.MongoDbModels
{
    public class MenuAccess
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UniqueId { get; set; }

        [BsonRequired]
        public string RoleId { get; set; }

        [BsonRequired]
        public string MenuId { get; set; }

        [BsonRequired]
        public TrueFalse IsAccess { get; set; }
    }
}
