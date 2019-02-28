using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;
using Servers.Models;
using Servers.Models.Interfaces;
using Servers.Services.Interfaces;

namespace Servers.Services.WorldServices
{
    public class WorldService: IWorldService
    {
        private IWorld World { get; set; }

        public WorldService(IWorld world)
        {
            World = world;
        }

        public ReturnCode AddNewPlayerToWorld(IPlayer player)
        {
            return World.AddPlayer(player);
        }

        public IRegion GetRegionForPlayer(IPlayer player)
        {
           // return World.GetRegion();
            return new Region(1,2);
        }
    }
}
