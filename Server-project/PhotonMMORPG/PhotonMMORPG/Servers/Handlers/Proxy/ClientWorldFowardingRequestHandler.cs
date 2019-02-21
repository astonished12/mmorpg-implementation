using System.Linq;
using GameCommon;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Support;
using ExitGames.Logging;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Server;
using Servers.Config;
using Servers.Data.Client;
using ServiceStack.Redis;

namespace Servers.Handlers.Proxy
{
    public class ClientWorldFowardingRequestHandler : IHandler<IClientPeer>
    {
        private IClientCodeRemover CodeRemove { get; set; }
        private ILogger Log { get; set; }
        private IServerConnectionCollection<IServerType, IServerPeer> ConnectionCollection { get; set; }
        private IRedisPubSubServer RedisPubSub { get; set; }

        public ClientWorldFowardingRequestHandler(ILogger log, IClientCodeRemover codeRemover, 
            IServerConnectionCollection<IServerType, IServerPeer> connectionCollection,
            IRedisPubSubServer redisPubSubServer)
        {
            CodeRemove = codeRemover;
            Log = log;
            ConnectionCollection = connectionCollection;
            RedisPubSub = redisPubSubServer;
        }

        public MessageType Type => MessageType.Async | MessageType.Request | MessageType.Response;
        public byte Code => (byte)MessageOperationCode.World;
        public int? SubCode => null;

        public bool HandleMessage(IMessage message, IClientPeer peer)
        {
            Log.DebugFormat("Received World Message to forward");
            var messageForwarded = false;
            //Remove all codes that might attempt to be spoofed by the player (no hacking)
            CodeRemove.RemoveCodes(message);
            Log.DebugFormat("Remove code from message");
            //get a list of all appropriate servers -- assume only one world server;
            var worldServer = ConnectionCollection.GetServersByType<IServerPeer>(ServerType.WorldServer);
            Log.DebugFormat("Found {0} world servers", 
                worldServer.Count);


            //Add in any additional data we need before sending the message - the actual peer id of the client, any other data
            AddMessageData(message, peer);
            var worldSrv = worldServer.FirstOrDefault();
            if (worldSrv != null)
            {
                worldSrv.SendMessage(message);
                Log.DebugFormat("Forwarded the message there");
                messageForwarded = true;
            }
            return messageForwarded;
        }

        private void AddMessageData(IMessage message, IClientPeer peer)
        {
            //ensure the actual peer if of this client is sent forward
            message.Parameters.Add((byte)MessageParameterCode.PeerId, peer.PeerId.ToByteArray());
            Log.DebugFormat("Added the following peer id {0} to message",peer.PeerId);
            message.Parameters.Add((byte)MessageParameterCode.Object, MessageSerializerService.SerializeObjectOfType(peer.ClientData<CharacterData>()));
            Log.DebugFormat("Added character data to message");
        }
    }
}
