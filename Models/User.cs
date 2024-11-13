
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Restapi_Pluszpont.Models;

public class User
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; } = "";
    public User[]? Friends { get; set; }
    public string Secret { get; set; } = "";


}