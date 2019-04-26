using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using GameCommon;
using MultiplayerGameFramework.Implementation.Config;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using MultiplayerGameFramework.Interfaces.Support;
using Servers.Config;
using Servers.Data.Client;

namespace Servers.Handlers.World
{
    public class RegionOperationsResponseHandler: IHandler<IServerPeer>
    {
        public MessageType Type => MessageType.Response;

        public byte Code => (byte)MessageOperationCode.World;

        public int? SubCode => null;

        public ILogger Log { get; private set; }

        private readonly IConnectionCollection<IClientPeer> _connectionCollection;
        private readonly ServerConfiguration _serverConfiguration;
        private readonly IClientCodeRemover _clientCodeRemover;
        private IServerConnectionCollection<IServerType, IServerPeer> ServerConnectionCollection { get; set; }


        public RegionOperationsResponseHandler(ILogger log, IConnectionCollection<IClientPeer> connectionCollection, 
            IServerConnectionCollection<IServerType, IServerPeer> serverConnectionCollection,
            ServerConfiguration serverConfiguration, IClientCodeRemover clientCodeRemover)
        {
            Log = log;
            _connectionCollection = connectionCollection;
            ServerConnectionCollection = serverConnectionCollection;
            _serverConfiguration = serverConfiguration;
            _clientCodeRemover = clientCodeRemover;
        }

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            // Got a response back from the World - might be successful, might not.
            if (message.Parameters.ContainsKey(_serverConfiguration.PeerIdCode))
            {
                //the key to obtain the current server with null:)) good framework 100cc;))
                var worldServer = ServerConnectionCollection.GetServersByType<IServerPeer>(null).FirstOrDefault();

                if (worldServer != null)
                {
                    Log.DebugFormat("ON RegionOperationsResponseHandler:  Found Peer");

                    var response = message as Response;

                    if (response.ReturnCode == (short)ReturnCode.RegionAddedNewPlayer)
                    {
                       
                    }

                    // make one call to send the message back - One "exit point" for the message.
                    worldServer.SendMessage(response);
                }
            }
            return true;
        }
    }
}
