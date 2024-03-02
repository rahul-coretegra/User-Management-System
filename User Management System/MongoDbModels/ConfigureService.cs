using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace User_Management_System.MongoDbModels
{
    public class ConfigureService
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UniqueId { get; set; }

        [BsonRequired]
        public string ItemUniqueId { get; set; }

        [BsonRequired]
        public string ItemName { get; set; }

        [BsonRequired]
        public string ItemValue { get; set; }

        [BsonRequired]
        public string ServiceId { get; set; }

        public Service Service { get; set; }

        [BsonRequired]
        public TrueFalse IsConfigured { get; set; }
    }
}
