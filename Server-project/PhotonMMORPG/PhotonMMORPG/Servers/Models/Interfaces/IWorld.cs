using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;

namespace Servers.Models.Interfaces
{
    public interface IWorld
    {
        int WorldTick { get; }
        IRegion GetRegion(Guid id);
        ReturnCode AddPlayer(IPlayer player);
        ReturnCode RemovePlayer(IPlayer player);
    }

}
