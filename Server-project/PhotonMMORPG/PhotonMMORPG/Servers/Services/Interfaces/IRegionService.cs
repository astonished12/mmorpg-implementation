using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;
using Servers.Models;
using Servers.Models.Interfaces;

namespace Servers.Services.Interfaces
{
    public interface IRegionService
    {
        void AssignRegionToHandle(AreaRegion[] areaRegions);
        ReturnCode AddPlayer(IPlayer player);
        ReturnCode DeletePlayer(IPlayer player);

    }
}
