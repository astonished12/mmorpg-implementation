using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Support;
using ExitGames.Logging;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Server;
using Servers.Config;

namespace Servers.Handlers.Login
{
    public class ClientLoginFowardingRequestHandler : IHandler<IClientPeer>
    {
        private IClientCodeRemover CodeRemove { get; set; }
        private ILogger Log { get; set; }
        private IServerConnectionCollection<IServerType, IServerPeer> ConnectionCollection { get; set; }
        public ClientLoginFowardingRequestHandler(ILogger log, IClientCodeRemover codeRemover, IServerConnectionCollection<IServerType, IServerPeer> connectionCollection)
        {
            CodeRemove = codeRemover;
            Log = log;
            ConnectionCollection = connectionCollection;
        }

        public MessageType Type => MessageType.Async | MessageType.Request | MessageType.Response;
        public byte Code => (byte)MessageOperationCode.Login;
        public int? SubCode => null;
        public bool HandleMessage(IMessage message, IClientPeer peer)
        {
            Log.DebugFormat("Received Login Message to forward");
            var messageForwarded = false;
            //Remove all codes that might attempt to be spoofed by the player (no hacking)
            CodeRemove.RemoveCodes(message);
            Log.DebugFormat("Remove code from message");
            //get a list of all appropriate servers -- assume only one login server;
            var loginServer = ConnectionCollection.GetServersByType<IServerPeer>(ServerType.LoginServer);
            Log.DebugFormat("Found {0} login servers", 
                loginServer.Count);

            //Add in any additional data we need before sending the message - the actual peer id of the client, any other data
            AddMessageData(message, peer);
            var login = loginServer.FirstOrDefault();
            if (login != null)
            {
                login.SendMessage(message);
                Log.DebugFormat("Forwarded the message there");
                messageForwarded = true;
            }
            return messageForwarded;
        }

        private void AddMessageData(IMessage message, IClientPeer peer)
        {
            //ensure the actual peer if of this client is sent forward
            message.Parameters.Add((byte)MessageParameterCode.PeerId, peer.PeerId.ToByteArray());
        }
    }
}
