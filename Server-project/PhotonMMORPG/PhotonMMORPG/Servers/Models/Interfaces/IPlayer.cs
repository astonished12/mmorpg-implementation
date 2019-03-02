using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Server;
using Photon.SocketServer;

namespace Servers.Models.Interfaces
{
    public interface IPlayer
    {
        IClientPeer Client { get; set; }
        IServerPeer ServerPeer { get; set; }
        int UserId { get; set; }
        string Name { get; set; }
    }
}
