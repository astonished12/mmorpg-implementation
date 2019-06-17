using System.Threading;
using Newtonsoft.Json;
using StackExchange.Redis;
using ILogger = ExitGames.Logging.ILogger;

namespace Servers.PubSubModels
{
    public class PlayerChannel
    {
        public string Name { get; set; }
        public Thread ChannelThread = null;
        private ILogger Log { get; set; }
        public ConnectionMultiplexer Client { get; set; }

        public PlayerChannel(ILogger log)
        {
            Log = log;
            Client = ConnectionMultiplexer.Connect("localhost");
        }

        public void SetChannel(string name)
        {
            Name = name;
            ChannelThread = new Thread(delegate ()
            {
                var subscription = Client.GetSubscriber();
                subscription.Subscribe(Name, (channel, msg) =>
                {
                    Log.DebugFormat("Client  Received '{0}' from channel '{1}'", msg, channel);

                });
            });
            ChannelThread.Start();
        }

        public void SendInfoToMainClient(object message)
        {
            var publisher = Client.GetSubscriber();
            publisher.PublishAsync($"Client_{Name}", JsonConvert.SerializeObject(message, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            }));
        }

        public void SendInfoToOthersClients(object message)
        {
            var publisher = Client.GetSubscriber();
            publisher.PublishAsync($"Entity_{Name}", JsonConvert.SerializeObject(message, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            }));
        }


    }
}
