using Restapi_Pluszpont.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoApi.Models;
namespace Restapi_Pluszpont.Services;

public class BetterUserService
{
    private readonly IMongoCollection<BetterUser> _userCollection;

    public BetterUserService(
        IOptions<BetterUserDatabaseSettings> userDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            userDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            userDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<BetterUser>(
            userDatabaseSettings.Value.BetterUsersCollectionName);
    }

    public async Task<List<BetterUser>> GetAsync() =>
        await _userCollection.Find(_ => true).ToListAsync();

    public async Task<BetterUser?> GetAsync(string id) =>
        await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(BetterUser newUser) =>
        await _userCollection.InsertOneAsync(newUser);

    public async Task UpdateAsync(string id, BetterUser updatedBook) =>
        await _userCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task RemoveAsync(string id) =>
        await _userCollection.DeleteOneAsync(x => x.Id == id);
}
