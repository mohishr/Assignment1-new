using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Assignment1.API.Models
{
    [BsonIgnoreExtraElements]
    public class MachineMongoModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Type { get; set; }
        [BsonElement("Assets")]
        public List<Asset> Assets { get; set; }
    }
}
