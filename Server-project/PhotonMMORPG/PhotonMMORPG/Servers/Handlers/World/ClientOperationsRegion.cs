using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using GameCommon;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using MultiplayerGameFramework.Interfaces.Support;
using Servers.Config;

namespace Servers.Handlers.World
{
    public class ClientOperationsRegion : IHandler<IServerPeer>
    {
        private IClientCodeRemover CodeRemove { get; set; }
        private IServerConnectionCollection<IServerType, IServerPeer> ConnectionCollection { get; set; }
        private ILogger Log { get; set; }

        public ClientOperationsRegion(ILogger log, IClientCodeRemover codeRemover,
            IServerConnectionCollection<IServerType, IServerPeer> connectionCollection)
        {
            CodeRemove = codeRemover;
            ConnectionCollection = connectionCollection;
            Log = log;
        }

        public MessageType Type => MessageType.Request;

        public byte Code => (byte)MessageOperationCode.World;

        public int? SubCode => null;

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            Log.DebugFormat("Received Region operations message to forward");
            var messageForwarded = false;

            var regionServer = ConnectionCollection.GetServersByType<IServerPeer>(ServerType.RegionServer);
            Log.DebugFormat("Found {0} region servers",
                regionServer.Count);


            //Add in any additional data we need before sending the message - the actual peer id of the client, any other data
            //AddMessageData(message, peer);
            var messageToForward = new Request((byte)MessageOperationCode.Region, message.SubCode, message.Parameters);
            var regServer = regionServer.FirstOrDefault();
            if (regServer != null)
            {
                regServer.SendMessage(messageToForward);
                Log.DebugFormat("Forwarded the message there");
                messageForwarded = true;
            }
            return messageForwarded;
        }
    }
}
