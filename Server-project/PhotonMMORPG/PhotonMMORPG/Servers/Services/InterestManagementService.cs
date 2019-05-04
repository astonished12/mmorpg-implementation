using System.Diagnostics;
using System.Linq;
using ExitGames.Logging;
using MultiplayerGameFramework.Interfaces.Client;
using Photon.MmoDemo.Common;
using Servers.Models.Interfaces;
using Servers.Services.Interfaces;

namespace Servers.Services
{
    public class InterestManagementService : IInterestManagementService
    {
        private IRegion Region { get; set; }
        private IRegionService RegionService { get; set; }
        private readonly Vector AOILengthOfPlayers = new Vector(20f, 0f, 20f);
        private ILogger Log { get; set; }

        public InterestManagementService(IRegion region, ILogger log)
        {
            Region = region;
            Log = log;
        }

        public void ComputeAreaOfInterest(IClientPeer peer, IPlayer player)
        {
            var playerPos = new Vector(player.Character.CharacterDataFromDb.Loc_X,
                0f, player.Character.CharacterDataFromDb.Loc_Z);

            var playerBox = new BoundingBox(playerPos - AOILengthOfPlayers, playerPos + AOILengthOfPlayers);
            //to do 
            foreach (var areaRegion in Region.AreaRegions)
            {
                foreach (var entity in areaRegion.Entities)
                {
                    //JUST ONE NPC CHARCTER REMEBER THAT
                    if (playerBox.Contains(entity.NpcCharacters[0].Position))
                    {
                        Log.DebugFormat("Player {0} discovered new object {1}", player.Name, entity.NpcCharacters[0].Position);
                    }
                }
            }

        }
    }
}
