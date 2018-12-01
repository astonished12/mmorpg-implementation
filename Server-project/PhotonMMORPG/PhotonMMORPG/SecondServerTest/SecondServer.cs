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

namespace SecondServerTest
{
    public class SecondServer : ApplicationBase
    {
        private readonly ILogger Log = LogManager.GetCurrentClassLogger();

        /*protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new Listener(initRequest);
        }

        protected override void Setup()
        {
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] =
                Path.Combine(this.ApplicationRootPath, "log");
            var file = new FileInfo(Path.Combine(BinaryPath, "log4net.config"));
            if (file.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);
            }

            Log.Debug("The second server is ready");

        }

        protected override void TearDown()
        {
            Log.Debug("Server is down");
        }

    }*/

        private MyOutboundPeer outboundPeer;


        protected override void Setup()
        {
            this.outboundPeer = new MyOutboundPeer(this);
            this.outboundPeer.ConnectTcp(new IPEndPoint(IPAddress.Parse("123.456.789.123"), 4520),
                "MyInboundApplication");
        }

        public void SendMessageToMaster(string message)
        {
            var parameters = new Dictionary<byte, object>();
            parameters[0] = message;
            this.outboundPeer.SendOperationRequest(new OperationRequest {OperationCode = 1, Parameters = parameters},
                new SendParameters());
        }

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            //if (initRequest.LocalPort == 4520)
            //{
                return new MyInboundPeer(initRequest);
            //}
        }

        protected override void TearDown()
        {
            Log.Debug("Server is down");
        }
    }
}
