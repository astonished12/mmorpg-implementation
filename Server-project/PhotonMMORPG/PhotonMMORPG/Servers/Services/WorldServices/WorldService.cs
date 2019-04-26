using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using GameCommon;
using MGF.Mappers;
using MultiplayerGameFramework.Implementation.Server;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Server;
using Photon.MmoDemo.Common;
using Servers.Config;
using Servers.Models;
using Servers.Models.Interfaces;
using Servers.Services.Interfaces;
using ServiceStack.Redis;

namespace Servers.Services.WorldServices
{
    public class WorldService : IWorldService
    {
        private IWorld World { get; set; }
        private IRedisPubSubServer WorldRedisPubSubServer { get; set; }
        private IRedisClientsManager ClientsManager { get; set; }
        private ILogger Log { get; set; }
        public bool RegionWasAssignedToRegionServers;
        private IServerConnectionCollection<IServerType, IServerPeer> ServerConnectionCollection { get; set; }

        public WorldService(IWorld world, IRedisPubSubServer worldRedisPubSubServer, IRedisClientsManager clientsManager,
            ILogger log, IServerConnectionCollection<IServerType, IServerPeer> serverConnectionCollection)
        {
            World = world;
            WorldRedisPubSubServer = worldRedisPubSubServer;
            ClientsManager = clientsManager;
            Log = log;
            ServerConnectionCollection = serverConnectionCollection;
        }

        public ReturnCode AddNewPlayerToWorld(IPlayer player)
        {
            return World.AddPlayer(player);
        }

        public IAreaRegion GetRegionForPlayer(IPlayer player)
        {
            /*to do: Check if player was in the world cache!!!!!!!!!!!!!!! 
                if it is get position from there and return region by the position !!!!!!!
                    else
                return the spawn Position on terrain A.K.A newPosition , add player to cache              
            then => emit to all world consumers that the player is here and is ready to join !!!!!!!!!!!!!!!!!!! (LATER)             
            */
            
            Vector pos;
            using (IRedisClient redis = ClientsManager.GetClient())
            {
                Log.DebugFormat("The redis client is working here on world server");
                if (redis.Get<Servers.Models.Character>(player.Name) == null)
                {
                    Log.Debug("Player isn't in cache");
                    var character = new Character()
                    {
                        CharacterDataFromDb = CharacterMapper.LoadByName(player.Name)
                    };
                    pos = new Vector() { X = character.CharacterDataFromDb.Loc_X, Y = character.CharacterDataFromDb.Loc_Y, Z = character.CharacterDataFromDb.Loc_Z };
                    redis.Set(player.Name, character);
                    Log.Debug("Player added to cache");
                }
                else
                {
                    var character = redis.Get<Character>(player.Name);
                    pos = new Vector() { X = character.CharacterDataFromDb.Loc_X, Y = character.CharacterDataFromDb.Loc_Y, Z = character.CharacterDataFromDb.Loc_Z };
                    Log.DebugFormat("The position from cache of player {0} is {1}", player.Name, pos);
                }
            }

            return World.GetRegion(pos);
        }

        public IWorld GetWorld()
        {
            return World;
        }


        public void AssignRegionServerToGameWorldRegion()
        {
            if (RegionWasAssignedToRegionServers == false)
            {
                World.GridWorld.SetRegionsToServers(ServerConnectionCollection);
                RegionWasAssignedToRegionServers = true;
            }
        }
    }
}
