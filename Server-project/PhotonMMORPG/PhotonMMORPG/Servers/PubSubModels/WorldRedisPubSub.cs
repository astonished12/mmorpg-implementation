using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servers.PubSubModels.Enums;
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
                Enum.TryParse("Active", out WorldPubSubMessageType messageType);

                switch (messageType)
                {
                    case WorldPubSubMessageType.NewPlayerInRegion:
                    case WorldPubSubMessageType.PlayerLeavesRegion:
                    case WorldPubSubMessageType.CrossRegion:
                        OnCrossRegion(msg);
                        break;
                    default:
                        break;
                }
            };
        }

        public void OnCrossRegion(string msg)
        {

        }

    }
}
