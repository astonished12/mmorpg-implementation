using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;

namespace Servers.Models.Interfaces
{
    public interface IRegion
    {
        List<IPlayer> ClientsInRegion { get; set; }
        AreaRegion[] AreaRegions { get; set; }
        ReturnCode AddPlayer(IPlayer player);
        ReturnCode RemovePlayer(IPlayer player);
    }
}
