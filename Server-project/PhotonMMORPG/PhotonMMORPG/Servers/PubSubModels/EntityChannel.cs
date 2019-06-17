using Newtonsoft.Json;
using StackExchange.Redis;

namespace Servers.PubSubModels
{
    public class EntityChannel
    {
        public string Name { get; set; }
        public ConnectionMultiplexer Client { get; set; }

        public EntityChannel(string name)
        {
            Name = name;
            Client = ConnectionMultiplexer.Connect("localhost");
        }

        public void SendUpdateNpcNotification(string message)
        {
            var publisher = Client.GetSubscriber();
            publisher.PublishAsync(Name, JsonConvert.SerializeObject(message, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            }));
        }
    }
}
