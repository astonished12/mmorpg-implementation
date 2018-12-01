using Photon.SocketServer;
using Photon.SocketServer.ServerToServer;
using PhotonHostRuntimeInterfaces;

namespace SecondServerTest
{
    internal class MyOutboundPeer: OutboundS2SPeer
    {
        public MyOutboundPeer(ApplicationBase application):base(application)
        {
            
        }

        protected override void OnConnectionEstablished(object responseObject)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnConnectionFailed(int errorCode, string errorMessage)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnEvent(IEventData eventData, SendParameters sendParameters)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnOperationResponse(OperationResponse operationResponse, SendParameters sendParameters)
        {
            throw new System.NotImplementedException();
        }
    }
}