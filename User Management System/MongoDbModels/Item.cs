using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace User_Management_System.MongoDbModels
{
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ItemId { get; set; }

        [BsonRequired]
        public string ItemName { get; set; }

        [BsonRequired]
        public string ItemValue { get; set; }
    }
}
