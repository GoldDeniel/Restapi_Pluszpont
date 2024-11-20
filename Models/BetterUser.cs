
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Restapi_Pluszpont.Models;

public class BetterUser
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; } = "";
    public string Secret { get; set; } = "";
    public List<string> FriendIds { get; set; } = new List<string>();


}