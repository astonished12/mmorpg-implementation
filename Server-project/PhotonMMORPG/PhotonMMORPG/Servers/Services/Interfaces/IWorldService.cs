using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Server;
using Servers.Models.Interfaces;

namespace Servers.Services.Interfaces
{
    public interface IWorldService
    {
        ReturnCode AddNewPlayerToWorld(IPlayer player);
        IAreaRegion GetRegionForPlayer(IPlayer player);
        IWorld GetWorld();
        void AssignRegionServerToGameWorldRegion();
        ReturnCode UpdatePositionAndRotationAndAnimation(IPlayer player, params object[] data);
        IPlayer GetPlayer(string name);
    }
}
