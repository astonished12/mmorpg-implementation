using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MGF_Photon;
using MGF_Photon.Implementation;
using MGF_Photon.Implementation.Server;
using MGF_Photon;
using MultiplayerGameFramework.Implementation.Config;
using Photon.SocketServer;
using Photon.SocketServer.ServerToServer;
using PhotonHostRuntimeInterfaces;

namespace MGF.Photon.Implementation.Server
{
    public class OutboundPhotonPeer : OutboundS2SPeer
    {
        private PhotonApplication _application;
        private PhotonServerPeer _serverPeer;
        private PeerInfo _peerInfo;

        public PhotonServerPeer ServerPeer
        {
            get => _serverPeer;
            set => _serverPeer = value;
        }

        public OutboundPhotonPeer(PhotonApplication application, PeerInfo peerInfo)
        : base(application)
        {
            _application = application;
            _peerInfo = peerInfo;
            SetPrivateCustomTypeCache(new TypeCache().GetCache());
        }

        protected override void OnConnectionEstablished(object responseObject)
        {
            ServerPeer =
                ((PhotonPeerFactory)_application.PeerFactory).ServerPeerFactory(this, _peerInfo.IsSiblingConnection);
            ServerPeer.Register();
        }

        protected override void OnConnectionFailed(int errorCode, string errorMessage)
        {
            ReconnectToPeer();
        }

        public void ReconnectToPeer()
        {
            _peerInfo.NumTries++;
            if (_peerInfo.NumTries < _peerInfo.MaxTries)
            {
                var timer = new Timer(o => ConnectTcp(_peerInfo.MasterEndPoint, _peerInfo.ApplicationName, TypeCache.SerializePeerInfo(_peerInfo)), null, _peerInfo.ConnectRetryIntervalSeconds * 1000, 0);
            }
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            _serverPeer.OnDisconnect(reasonCode, reasonDetail);
        }

        protected override void OnEvent(IEventData eventData, SendParameters sendParameters)
        {
            _serverPeer.OnEvent(eventData, sendParameters);
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            _serverPeer.OnOperationRequest(operationRequest, sendParameters);
        }

        protected override void OnOperationResponse(OperationResponse operationResponse, SendParameters sendParameters)
        {
            _serverPeer.OnOperationResponse(operationResponse, sendParameters);
        }
    }
}
