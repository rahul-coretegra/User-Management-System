using Microsoft.Extensions.Options;

namespace User_Management_System.MongoDbConfigurations
{
    public class MongoDbSettings
    {
        public string Client { get; set; }
        public string DatabaseName { get; set; }
        public IOptions<MongoDbSettings> Values { get; internal set; }
    }
}
