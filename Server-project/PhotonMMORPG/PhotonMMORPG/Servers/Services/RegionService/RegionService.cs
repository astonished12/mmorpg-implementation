using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;
using Servers.Models;
using Servers.Models.Interfaces;
using Servers.Services.Interfaces;

namespace Servers.Services.RegionService
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
        
       
    }
}
