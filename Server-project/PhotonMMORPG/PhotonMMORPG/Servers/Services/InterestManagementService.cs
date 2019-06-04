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
        private readonly Vector _aoiLengthOfPlayers = new Vector(70f, 0f, 70f);
        private ILogger Log { get; set; }
        private Dictionary<IClientPeer, List<ICharacter>> _playerAreaOfInterest;


        public InterestManagementService(IRegion region, ILogger log)
        {
            Region = region;
            Log = log;
            _playerAreaOfInterest = new Dictionary<IClientPeer, List<ICharacter>>();
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
                    _playerAreaOfInterest.TryGetValue(peer, out List<ICharacter> npcCharactersAreaOfInterestList);
                    if (null == npcCharactersAreaOfInterestList)
                    {
                        _playerAreaOfInterest.Add(peer, new List<ICharacter>());
                    }

                    if (entity.NpcCharacters.Count > 0)
                    {
                        var entityInPlayerAoi =
                        npcCharactersAreaOfInterestList?.FirstOrDefault(x => x.Equals(entity.NpcCharacters[0]));
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

                foreach (var playerInRegion in Region.ClientsInRegion)
                {
                    if (playerInRegion.Name.Equals(player.Name))
                        continue;

                    var playerInRegionPosition = new Vector
                    {
                        X = playerInRegion.Character.CharacterDataFromDb.Loc_X,
                        Y = 0,
                        Z = playerInRegion.Character.CharacterDataFromDb.Loc_Z
                    };

                    _playerAreaOfInterest.TryGetValue(peer, out List<ICharacter> entititiesAoi);
                    if (null == entititiesAoi)
                    {
                        _playerAreaOfInterest.Add(peer, new List<ICharacter>());
                    }
                    else
                    {
                        var charactersAoi = entititiesAoi.Where(x => x is Character);
                        var entityInPlayerAoi = charactersAoi?.FirstOrDefault(x => ((Character)x).CharacterDataFromDb.Name.Equals(playerInRegion.Name));
                        if (playerBox.Contains(playerInRegionPosition))
                        {
                            if (null != entityInPlayerAoi) continue;
                            _playerAreaOfInterest[peer].Add(playerInRegion.Character);
                            Log.DebugFormat("Player {0} discovered new player {1} at {2}", player.Name, playerInRegion.Name, playerInRegionPosition);
                        }
                        else
                        {
                            if (null == entityInPlayerAoi) continue;
                            var character = _playerAreaOfInterest[peer].Where(x => x is Character).SingleOrDefault(x =>
                                  ((Character)x).CharacterDataFromDb.Name.Equals(playerInRegion.Name));
                            if (null == character) continue;
                            _playerAreaOfInterest[peer].Remove(character);
                            Log.DebugFormat("Player {0} drop player {1} from his AreaOfInterest at {2}", player.Name, playerInRegion.Name,
                                playerInRegionPosition);
                        }
                    }


                }
            }

        }

        public IEnumerable<ICharacter> GetAreaOfInterest(IClientPeer peer)
        {
            _playerAreaOfInterest.TryGetValue(peer, out List<ICharacter> characters);
            return characters;
        }
    }
}
