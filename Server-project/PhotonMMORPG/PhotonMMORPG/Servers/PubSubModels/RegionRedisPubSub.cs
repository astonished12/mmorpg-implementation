using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servers.PubSubModels.Enums;
using ServiceStack.Redis;

namespace Servers.PubSubModels
{
    public class RegionRedisPubSub: RedisPubSubServer
    {

        public RegionRedisPubSub(IRedisClientsManager clientsManager, params string[] channels) : base(clientsManager, channels)
        {
            //clientsManager.GetClient().PublishMessage()
        }
    }
}
