using User_Management_System.ManagementModels.EnumModels;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace User_Management_System.MongoDbModels
{
    public class Service
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ServiceId { get; set; }

        [BsonRequired]
        public string ServiceUniqueId { get; set; }

        [BsonRequired]
        public string ServiceName { get; set; }

        [BsonRequired]
        public TypeOfService ServiceType { get; set; }

        [BsonRequired]
        public TrueFalse Status { get; set; }

    }
}
