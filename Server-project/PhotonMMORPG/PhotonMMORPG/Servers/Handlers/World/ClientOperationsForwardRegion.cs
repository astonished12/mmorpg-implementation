using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using GameCommon;
using MGF_Photon.Implementation.Data;
using MultiplayerGameFramework.Implementation.Client;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using MultiplayerGameFramework.Interfaces.Support;
using Servers.Config;
using Servers.Data.Client;
using Servers.Services.Interfaces;

namespace Servers.Handlers.World
{
    public class ClientOperationsForwardRegion : IHandler<IServerPeer>
    {
        private IClientCodeRemover CodeRemove { get; set; }
        private IServerConnectionCollection<IServerType, IServerPeer> ServerConnectionCollection { get; set; }
        private IConnectionCollection<IClientPeer> ClientConnectionCollection { get; set; }
        private ILogger Log { get; set; }
        private IWorldService WorldService { get; set; }

        public ClientOperationsForwardRegion(ILogger log, IClientCodeRemover codeRemover,
            IServerConnectionCollection<IServerType, IServerPeer> serverConnectionCollection,
            IConnectionCollection<IClientPeer> clientConnectionCollection,
            IWorldService worldService)
        {
            CodeRemove = codeRemover;
            ServerConnectionCollection = serverConnectionCollection;
            ClientConnectionCollection = clientConnectionCollection;
            Log = log;
            WorldService = worldService;
        }

        public MessageType Type => MessageType.Request;

        public byte Code => (byte)MessageOperationCode.World;

        public int? SubCode => null;

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            Log.DebugFormat("Received Region operations message to forward");
            var messageForwarded = false;

            var regionServers = ServerConnectionCollection.GetServersByType<IServerPeer>(ServerType.RegionServer);
            Log.DebugFormat("Found {0} region servers",
                regionServers.Count);

            var clientPeerGuid = new Guid((byte[])message.Parameters[(byte)MessageParameterCode.PeerId]);
            var clientData = ClientConnectionCollection.GetPeers<IClientPeer>()
                .FirstOrDefault(x => x.PeerId == clientPeerGuid).ClientData<CharacterData>();

            var regionServerNameBasedOnClientRegion = clientData.Region;

            //Add in any additional data we need before sending the message - the actual peer id of the client, any other data
            //AddMessageData(message, clientPeerGuid);
            var regServer = regionServers.FirstOrDefault(x=>x.ServerData<ServerData>().ApplicationName.Equals(regionServerNameBasedOnClientRegion.ApplicationServerName));
            if (regServer != null)
            {
                regServer.SendMessage(message);
                Log.DebugFormat("Forwarded the message to region server {0} ", regServer.Server.ApplicationName);
                messageForwarded = true;
            }

            return messageForwarded;
        }

        private void AddMessageData(IMessage message, Guid clientPeerGuid)
        {
            
        }
    }
}
