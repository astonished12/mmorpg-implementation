using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace ProxyServer
{
    public class ProxyServer: ApplicationBase
    {
        private readonly ILogger Log = LogManager.GetCurrentClassLogger();

       protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            ConnectToServerTcp(new IPEndPoint(IPAddress.Parse("123.456.789.123"), 4520), "MyMasterApplication", null);
            return new ProxyPeer(initRequest);
        }

        protected override void Setup()
        {
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            var file = new FileInfo(Path.Combine(BinaryPath, "log4net.config"));
            if (file.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);
            }

            Log.Debug("Proxy Server is ready");

        }

        protected override void TearDown()
        {
            Log.Debug("Proxy server is down");
        }
    }
}
