using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace ProxyServer
{
    public class ProxyPeer:ClientPeer
    {
        public ProxyPeer(InitRequest initRequest)
            : base(initRequest)
        {
        }

        protected override void OnDisconnect(DisconnectReason disconnectCode, string reasonDetail)
        {
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
        }
    }
}
