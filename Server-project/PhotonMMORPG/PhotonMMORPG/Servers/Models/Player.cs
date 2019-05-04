using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Server;
using Servers.Models.Interfaces;

namespace Servers.Models
{
    public class Player : IPlayer
    {
        public Guid ClientPeerId { get; set; }
        public IServerPeer ServerPeer { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public IWorld World { get; set; }
        public Character Character { get; set; }

    }
}
