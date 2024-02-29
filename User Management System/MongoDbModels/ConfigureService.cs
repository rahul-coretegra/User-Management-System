using User_Management_System.ManagementModels.EnumModels;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace User_Management_System.MongoDbModels
{
    public class ConfigureService
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UniqueId { get; set; }

        [BsonRequired]
        public string ServiceUniqueId { get; set; }

        [BsonRequired]
        public string ServiceName { get; set; }

        [BsonRequired]
        public TypeOfService ServiceType { get; set; }

        public List<Item> Items { get; set; }

    }
}
