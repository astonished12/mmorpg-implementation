using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ExitGames.Logging;
using GameCommon;
using MultiplayerGameFramework.Interfaces.Client;
using Servers.Models;
using Servers.Models.Interfaces;
using Servers.Services.Interfaces;

namespace Servers.Services
{
    public class InterestManagementService : IInterestManagementService
    {
        private IRegion Region { get; set; }
        private IRegionService RegionService { get; set; }
        private readonly Vector _aoiLengthOfPlayers = new Vector(20f, 0f, 20f);
        private ILogger Log { get; set; }
        private Dictionary<IClientPeer, List<NpcCharacter>> _playerAreaOfInterest;

        public InterestManagementService(IRegion region, ILogger log)
        {
            Region = region;
            Log = log;
            _playerAreaOfInterest = new Dictionary<IClientPeer, List<NpcCharacter>>();
        }

        public void ComputeAreaOfInterest(IClientPeer peer, IPlayer player)
        {
            var playerPos = new Vector(player.Character.CharacterDataFromDb.Loc_X,
                0f, player.Character.CharacterDataFromDb.Loc_Z);

            var playerBox = new BoundingBox(playerPos - _aoiLengthOfPlayers, playerPos + _aoiLengthOfPlayers);
            //to do 
            foreach (var areaRegion in Region.AreaRegions)
            {
                foreach (var entity in areaRegion.Entities)
                {
                    _playerAreaOfInterest.TryGetValue(peer, out List<NpcCharacter> npcCharactersAreaOfInterestList);
                    if (null == npcCharactersAreaOfInterestList)
                    {
                        _playerAreaOfInterest.Add(peer, new List<NpcCharacter>());
                    }

                    NpcCharacter entityInPlayerAoi = npcCharactersAreaOfInterestList?.FirstOrDefault(x => x.Equals(entity.NpcCharacters[0]));
                    //JUST ONE NPC CHARCTER REMEBER THAT
                    if (playerBox.Contains(entity.NpcCharacters[0].Position))
                    {
                        if (null != entityInPlayerAoi) continue;
                        _playerAreaOfInterest[peer].Add(entity.NpcCharacters[0]);
                        Log.DebugFormat("Player {0} discovered new object {1}", player.Name,
                            entity.NpcCharacters[0].Position);
                    }
                    else
                    {
                        if (null == entityInPlayerAoi) continue;
                        _playerAreaOfInterest[peer].Remove(entity.NpcCharacters[0]);
                        Log.DebugFormat("Player {0} drop object {1} from his AreaOfInterest", player.Name,
                            entity.NpcCharacters[0].Position);
                    }

                }
            }

        }

        public List<NpcCharacter> GetAreaOfInterest(IClientPeer peer)
        {
            _playerAreaOfInterest.TryGetValue(peer, out List<NpcCharacter> npcCharacters);
            return npcCharacters;
        }
    }
}
