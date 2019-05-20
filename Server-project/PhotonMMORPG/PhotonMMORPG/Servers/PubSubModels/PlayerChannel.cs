using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Core;
using ServiceStack;
using ServiceStack.Redis;
using ILogger = ExitGames.Logging.ILogger;

namespace Servers.PubSubModels
{
    public class PlayerChannel
    {
        public string Name { get; set; }
        private RedisClient Client { get; set; }
        public Thread ChannelThread = null;
        private ILogger Log { get; set; }

        public PlayerChannel(ILogger log)
        {
            Log = log;
            Client = new RedisClient("localhost: 6379");
        }

        public void SetChannel(string name)
        {
            Name = name;
            ChannelThread = new Thread(delegate ()
            {
                IRedisSubscription subscription = null;

                using (subscription = Client.CreateSubscription())
                {
                    subscription.OnSubscribe = channel => { Log.DebugFormat("Client Subscribed to '{0}'", channel); };
                    subscription.OnUnSubscribe = channel =>
                    {
                        Log.DebugFormat("Client #{0} UnSubscribed from ", channel);
                    };
                    subscription.OnMessage = (channel, msg) =>
                    {
                        Log.DebugFormat("Client  Received '{0}' from channel '{1}'", msg, channel);
                    };
                }

                subscription.SubscribeToChannels($"Server_{Name}");
            });
            ChannelThread.Start();
        }

        public void SendNotification(object message)
        {
            using (var client = new RedisClient("localhost: 6379"))
            {
                client.PublishMessage($"Client_{Name}", message.ToJson());
            }
        }
    }
}
