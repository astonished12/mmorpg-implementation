using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace SecondServerTest
{
    public class Listener : ClientPeer
    {
        private readonly ILogger Log = LogManager.GetCurrentClassLogger();
        public string CharacterName { get; private set; }
       
        
        public Listener(InitRequest initRequest) : base(initRequest)
        {
            Log.Info("Player connection ip: " + initRequest.RemoteIP);
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            throw new NotImplementedException();
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            throw new NotImplementedException();
        }
    }
}
