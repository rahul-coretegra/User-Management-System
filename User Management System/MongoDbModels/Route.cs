using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using User_Management_System.ManagementModels.EnumModels;

namespace User_Management_System.MongoDbModels
{
    public class Route
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string RouteId { get; set; }

        [BsonRequired]
        public string RoutePath { get; set; }

        [BsonRequired]
        public string RouteName { get; set; }

        [BsonRequired]
        public TrueFalse Status { get; set; }


    }
}
