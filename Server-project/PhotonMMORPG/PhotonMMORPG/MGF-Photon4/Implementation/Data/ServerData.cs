using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Interfaces.Server;

namespace MGF_Photon.Implementation.Data
{
    public class ServerData : IServerData
    {
        public Guid? ServerId { get; set; }
        public string TcpAddress { get; set; }
        public string UdpAddress { get; set; }
        public string ApplicationName { get; set; }
        public int ServerType { get; set; }
    }
}
