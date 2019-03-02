using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using GameCommon;
using Servers.Models;
using Servers.Models.Interfaces;
using Servers.Services.Interfaces;
using ServiceStack.Redis;

namespace Servers.Services.WorldServices
{
    public class WorldService: IWorldService
    {
        private IWorld World { get; set; }
        private IRedisPubSubServer WorldRedisPubSubServer { get; set; }
        private IRedisClientsManager ClientsManager { get; set; }
        private ILogger Log { get; set; }

        public WorldService(IWorld world, IRedisPubSubServer worldRedisPubSubServer, IRedisClientsManager clientsManager, ILogger log)
        {
            World = world;
            WorldRedisPubSubServer = worldRedisPubSubServer;
            ClientsManager = clientsManager;
            Log = log;
        }

        public ReturnCode AddNewPlayerToWorld(IPlayer player)
        {
            return World.AddPlayer(player);
        }

        public IRegion GetRegionForPlayer(IPlayer player)
        {
            /*to do: Check if player was in the world cache!!!!!!!!!!!!!!! 
                if it is get position from there and return region by the position !!!!!!!
                    else
                return the spawn Position on terrain A.K.A newPosition , add player to cache              
            then => emit to all world consumers that the player is here and is ready to join !!!!!!!!!!!!!!!!!!! (LATER)             
            */

            using (IRedisClient redis = ClientsManager.GetClient())
            {
                Log.DebugFormat("The redis client is working here on world server");
                if (redis.Get<string>(nameof(player.Name)) == null)
                {
                    Log.Debug("Player isn't in cache");
                    redis.Set(nameof(player.Name), player.Name);
                    Log.Debug("Player added to cache");
                }


            }

            return new Region(1,2);
        }

        public IWorld GetWorld()
        {
            return World;
        }
    }
}
