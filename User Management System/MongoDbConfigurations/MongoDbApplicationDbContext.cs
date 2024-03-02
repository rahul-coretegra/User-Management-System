using Microsoft.Extensions.Options;
using MongoDB.Driver;
using User_Management_System.MongoDbModels;

namespace User_Management_System.MongoDbConfigurations
{
    public class MongoDbApplicationDbContext
    {
        public IMongoDatabase Database;
        public MongoDbApplicationDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.Client);
            Database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<User> User => Database.GetCollection<User>("User");
        public IMongoCollection<UserRole> UserRole => Database.GetCollection<UserRole>("UserRole");
        public IMongoCollection<RoleAccess> RoleAccess => Database.GetCollection<RoleAccess>("RoleAccess");
        public IMongoCollection<MongoDbModels.Route> Route => Database.GetCollection<MongoDbModels.Route>("Route");
        public IMongoCollection<RouteAccess> RouteAccess => Database.GetCollection<RouteAccess>("RouteAccess");
        public IMongoCollection<Menu> Menu => Database.GetCollection<Menu>("Menu");
        public IMongoCollection<MenuAccess> MenuAccess => Database.GetCollection<MenuAccess>("MenuAccess");
        public IMongoCollection<Service> Service => Database.GetCollection<Service>("Service");

    }
}
