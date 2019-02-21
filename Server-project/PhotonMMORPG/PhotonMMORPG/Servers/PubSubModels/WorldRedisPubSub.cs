using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace Servers.PubSubModels
{
    public class WorldRedisPubSub: RedisPubSubServer
    {
        public WorldRedisPubSub(IRedisClientsManager clientsManager, params string[] channels) : base(clientsManager, channels)
        {
            OnMessage = (channel, msg) =>
            {
                OnCrossRegion(msg);
            };
        }

        public void OnCrossRegion(string msg)
        {

        }

    }
}
