using System.Linq;
using GameCommon;
using Servers.Models;
using Servers.Models.Interfaces;
using Servers.Services.Interfaces;

namespace Servers.Services
{
    public class RegionService: IRegionService
    {
        public IRegion Region { get; set; }

        public RegionService(IRegion region)
        {
            Region = region;
        }

        public void AssignRegionToHandle(AreaRegion[] areaRegions)
        {
            Region.AreaRegions = areaRegions;
        }

        public ReturnCode AddPlayer(IPlayer player)
        {
            return Region.AddPlayer(player);
        }

        public ReturnCode DeletePlayer(IPlayer player)
        {
            return Region.RemovePlayer(player);
        }

        public IPlayer GetPlayer(string name)
        {
            return Region.ClientsInRegion.FirstOrDefault(x => x.Name == name);
        }
    }
}
