using Photon.SocketServer;
using Photon.SocketServer.ServerToServer;
using PhotonHostRuntimeInterfaces;

namespace SecondServerTest
{
    public class MyInboundPeer : InboundS2SPeer
    {
        public MyInboundPeer(InitRequest initRequest)
            : base(initRequest)
        {
        }
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            // implement this to receive operation data
        }
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
        }

        protected override void OnEvent(IEventData eventData, SendParameters sendParameters)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnOperationResponse(OperationResponse operationResponse, SendParameters sendParameters)
        {
            throw new System.NotImplementedException();
        }
    }
}