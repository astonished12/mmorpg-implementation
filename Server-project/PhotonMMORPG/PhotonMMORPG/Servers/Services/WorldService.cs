using ExitGames.Logging;
using GameCommon;
using MGF.Mappers;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Server;
using Servers.Models;
using Servers.Models.Interfaces;
using Servers.Services.Interfaces;
using ServiceStack.Redis;

namespace Servers.Services
{
    public class WorldService : IWorldService
    {
        private IWorld World { get; set; }
        private IRedisClientsManager ClientsManager { get; set; }
        private ILogger Log { get; set; }
        public bool RegionWasAssignedToRegionServers;
        private IServerConnectionCollection<IServerType, IServerPeer> ServerConnectionCollection { get; set; }
        private ICacheService CacheService { get; set; }

        public WorldService(IWorld world, IRedisClientsManager clientsManager,
            ILogger log, IServerConnectionCollection<IServerType, IServerPeer> serverConnectionCollection, ICacheService cacheService)
        {
            World = world;
            ClientsManager = clientsManager;
            Log = log;
            ServerConnectionCollection = serverConnectionCollection;
            CacheService = cacheService;
        }

        public ReturnCode AddNewPlayerToWorld(IPlayer player)
        {
            return World.AddPlayer(player);
        }

        public IAreaRegion GetRegionForPlayer(IPlayer player)
        {
            Vector pos;
            var characterFromCache = CacheService.GetCharacterByName(player.Name);
            if (null == characterFromCache)
            {
                Log.Debug("Player isn't in cache");
                var character = new Character()
                {
                    CharacterDataFromDb = CharacterMapper.LoadByName(player.Name)
                };
                pos = new Vector() { X = character.CharacterDataFromDb.Loc_X, Y = character.CharacterDataFromDb.Loc_Y, Z = character.CharacterDataFromDb.Loc_Z };
                CacheService.AddOrUpdateCharacter(player.Name, character);
                Log.Debug("Player added to cache");
            }
            else
            {
                pos = new Vector() { X = characterFromCache.CharacterDataFromDb.Loc_X, Y = characterFromCache.CharacterDataFromDb.Loc_Y, Z = characterFromCache.CharacterDataFromDb.Loc_Z };
                Log.DebugFormat("The position from cache of player {0} is {1}", player.Name, pos);
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

        public ReturnCode UpdatePositionAndRotation(IPlayer player, params object[] data)
        {
            if (World.UpdatePlayerPositionAndRotation(player, data) == ReturnCode.Ok)
            {
                var character = CacheService.GetCharacterByName(player.Name);
                character.CharacterDataFromDb.Loc_X = (float)data[0];
                character.CharacterDataFromDb.Loc_Y = (float)data[1];
                character.CharacterDataFromDb.Loc_Z = (float)data[2];
                character.CharacterDataFromDb.Rot_X = (float)data[3];
                character.CharacterDataFromDb.Rot_Y = (float)data[4];
                character.CharacterDataFromDb.Rot_Z = (float)data[5];

                CacheService.AddOrUpdateCharacter(player.Name, character);
                return ReturnCode.Ok;
            }
            else
            {
                return ReturnCode.OperationInvalid;
            }
        }

        public IPlayer GetPlayer(string name)
        {
            return World.GetPlayer(name);
        }
    }
}
