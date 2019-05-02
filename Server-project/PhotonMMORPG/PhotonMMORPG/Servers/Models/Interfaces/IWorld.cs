using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;
using Photon.MmoDemo.Common;

namespace Servers.Models.Interfaces
{
    public interface IWorld
    {
        GridWorld GridWorld { get; }
        IAreaRegion GetRegion(Vector pos);
        ReturnCode AddPlayer(IPlayer player);
        IPlayer GetPlayer(string name);
        ReturnCode RemovePlayer(IPlayer player);
        ReturnCode UpdatePlayerPositionAndRotation(IPlayer player, params object[] data);
    }

}
