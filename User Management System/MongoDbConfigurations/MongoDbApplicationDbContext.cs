using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace User_Management_System.MongoDbConfigurations
{
    public class MongoDbApplicationDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbApplicationDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.MongoDbConnection);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }
    }
}
