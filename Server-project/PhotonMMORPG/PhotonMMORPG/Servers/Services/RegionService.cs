using System.Collections.Generic;
using System.Linq;
using ExitGames.Logging;
using GameCommon;
using Servers.Models;
using Servers.Models.Interfaces;
using Servers.PubSubModels;
using Servers.Services.Interfaces;

namespace Servers.Services
{
    public class RegionService: IRegionService
    {
        public IRegion Region { get; set; }
        public List<PlayerChannel> Channels { get; set; }
        private ILogger Log { get; set; }

        public RegionService(IRegion region, ILogger log)
        {
            Region = region;
            Channels = new List<PlayerChannel>();
            Log = log;
        }

        public void AssignRegionToHandle(AreaRegion[] areaRegions)
        {
            Region.AreaRegions = areaRegions;
        }

        public ReturnCode AddPlayer(IPlayer player)
        {
            var returnCode = Region.AddPlayer(player);
            if (returnCode == ReturnCode.RegionAddedNewPlayer)
            {
                var playerChannel = new PlayerChannel(Log);
                Channels.Add(playerChannel);
                playerChannel.SetChannel(player.Character.CharacterDataFromDb.Name);
            }

            return returnCode;
        }

        public ReturnCode DeletePlayer(IPlayer player)
        {
            var returnCode = Region.RemovePlayer(player);
            if (returnCode == ReturnCode.Ok)
            {
                var playerChannel = Channels.FirstOrDefault(x => x.Name == player.Character.CharacterDataFromDb.Name);
                if (playerChannel != null)
                {
                    Channels.Remove(playerChannel);
                    playerChannel.ChannelThread.Abort(); //ce plm
                }
            }

            return returnCode;
        }

        public IPlayer GetPlayer(string name)
        {
            return Region.ClientsInRegion.FirstOrDefault(x => x.Name == name);
        }

        public void AssignCharactersFromTemplate()
        {
            foreach (var regionAreaRegion in Region.AreaRegions)
            {
                regionAreaRegion.AssignCharactersFromNpcTemplate();
            }
        }

        public PlayerChannel GetPlayerChannel(string name)
        {
            return Channels?.FirstOrDefault(x => x.Name == name);
        }
    }
}
