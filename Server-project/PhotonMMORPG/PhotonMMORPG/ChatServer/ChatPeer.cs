using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace ChatServer
{
    class ChatPeer : ClientPeer
    {
    public ChatPeer(InitRequest initRequest)
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
