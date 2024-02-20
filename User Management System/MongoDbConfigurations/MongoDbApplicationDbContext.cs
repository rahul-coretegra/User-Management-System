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
        public IMongoCollection<UserAndRoles> UserAndRoles => Database.GetCollection<UserAndRoles>("UserAndRoles");
        public IMongoCollection<MongoDbModels.Route> Route => Database.GetCollection<MongoDbModels.Route>("Route");
        public IMongoCollection<RoleAndAccess> RoleAndAccess => Database.GetCollection<RoleAndAccess>("RoleAndAccess");
        public IMongoCollection<Menu> Menu => Database.GetCollection<Menu>("Menu");
        public IMongoCollection<RoleAndMenus> RoleAndMenus => Database.GetCollection<RoleAndMenus>("RoleAndMenus");


    }
}
