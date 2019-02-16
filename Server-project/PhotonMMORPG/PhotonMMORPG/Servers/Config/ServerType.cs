using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Interfaces.Config;

namespace Servers.Config
{
    public class ServerType : IServerType
    {
        public static ServerType LoginServer = new ServerType() { Name = "Login" };
        public static ServerType ProxyServer = new ServerType() { Name = "Proxy" };
        public static ServerType WorldServer = new ServerType() { Name = "World" };
        public static ServerType RegionServer = new ServerType() { Name = "Region" };
        public string Name { get; set; }

        public IServerType GetServerType(int serverType)
        {
            IServerType server = null;
            switch (serverType)
            {
                case 1:
                    server = LoginServer;
                    break;
                case 2:
                    server = WorldServer;
                    break;
                case 3:
                    server = RegionServer;
                    break;
                case 0:
                default:
                    server = ProxyServer;
                    break;
            }
            return server;
        }
    }
}
