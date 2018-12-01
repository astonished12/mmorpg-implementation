using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace LoginServer
{
    public class LoginPeer : ClientPeer
    {
        public LoginPeer(InitRequest initRequest)
            : base(initRequest)
        {
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            throw new System.NotImplementedException();
        }
    }
}