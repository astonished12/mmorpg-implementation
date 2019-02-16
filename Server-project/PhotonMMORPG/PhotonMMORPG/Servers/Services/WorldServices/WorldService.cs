using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servers.Models;
using Servers.Models.Interfaces;
using Servers.Services.Interfaces;

namespace Servers.Services.WorldServices
{
    public class WorldService: IWorldService
    {
        public void AddNewPlayerToWorld(IPlayer player)
        {
            World.Instance.AddPlayer(player);
        }
    }
}
