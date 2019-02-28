using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;
using Servers.Models.Interfaces;

namespace Servers.Services.Interfaces
{
    public interface IWorldService
    {
        ReturnCode AddNewPlayerToWorld(IPlayer player);
        IRegion GetRegionForPlayer(IPlayer player);
    }
}
