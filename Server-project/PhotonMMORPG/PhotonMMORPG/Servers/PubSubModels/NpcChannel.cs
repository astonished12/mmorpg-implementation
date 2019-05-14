using ServiceStack.Redis;

namespace Servers.PubSubModels
{
    public class NpcChannel
    {
        public IRedisClientsManager ClientsManager { get; set; }
        public string Name { get; set; }

        public NpcChannel(string name)
        {
            Name = name;
            ClientsManager = new BasicRedisClientManager("localhost:6379");
        }

        public void SendUpdateNpcNotification(string message)
        {
            ClientsManager.GetClient().PublishMessage(Name, message);
        }
    }
}
