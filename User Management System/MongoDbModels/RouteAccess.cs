using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.MongoDbModels
{
    public class RouteAccess
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UniqueId { get; set; }

        [BsonRequired]
        public string RoleId { get; set; }

        [BsonRequired]
        public string RouteId { get; set; }

        [BsonRequired]
        public TrueFalse IsAccess { get; set; }
    }
}
