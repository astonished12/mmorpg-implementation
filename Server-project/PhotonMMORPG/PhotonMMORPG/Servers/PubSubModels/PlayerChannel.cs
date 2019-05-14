using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Core;
using ServiceStack.Redis;
using ILogger = ExitGames.Logging.ILogger;
using LogManager = ExitGames.Logging.LogManager;

namespace Servers.PubSubModels
{
    public class PlayerChannel
    {
        public string Name { get; set; }
        private RedisClient ClientSub { get; set; }
        private RedisClient ClientPub { get; set; }
        public Thread ChannelThread = null;
        private ILogger Log { get; set; }

        public PlayerChannel(ILogger log)
        {
            Log = log;
            ClientSub = new RedisClient("localhost: 6379");
            ClientPub = new RedisClient("localhost: 6379");
        }

        public void SetChannel(string name)
        {
            Name = name;
            ChannelThread = new Thread(delegate ()
            {
                using (var subscription = ClientSub.CreateSubscription())
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
            });
            ChannelThread.Start();
        }

        public void SendNotification(string message)
        {
            ClientPub.PublishMessage(Name, message);
        }


    }
}
