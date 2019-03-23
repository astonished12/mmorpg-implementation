using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Logging;
using GameCommon;
using MultiplayerGameFramework.Implementation.Config;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using MultiplayerGameFramework.Interfaces.Support;
using Servers.Data.Client;
using Servers.Models;
using Servers.Services.Interfaces;
using ServiceStack.Redis;

namespace Servers.Handlers.World
{
    public class ClientEnterWorld : IHandler<IServerPeer>
    {
        private ILogger Log { get; set; }
        private IWorldService WorldService { get; set; }
        private IConnectionCollection<IClientPeer> ConnectionCollection { get; set; }
        private IRedisClientsManager ClientsManager { get; set; }
        private IRedisPubSubServer RedisPubSub { get; set; }
        private IPeerFactory PeerFactory;

        public ClientEnterWorld(ILogger log, IWorldService worldService, IConnectionCollection<IClientPeer> connectionCollection,
            IRedisClientsManager clientsManager, IRedisPubSubServer redisPubSubServer,
            IPeerFactory peerFactory)
        {
            Log = log;
            WorldService = worldService;
            ConnectionCollection = connectionCollection;
            ClientsManager = clientsManager;
            RedisPubSub = redisPubSubServer;
            PeerFactory = peerFactory;
        }

        public MessageType Type => MessageType.Request;

        public byte Code => (byte)MessageOperationCode.World;

        public int? SubCode => (int?)MessageSubCode.EnterWorld;

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            var playerData = MessageSerializerService.DeserializeObjectOfType<CharacterData>(message.Parameters[(byte)MessageParameterCode.Object]);

            var clientPeerGuid = new Guid((byte[])message.Parameters[(byte)MessageParameterCode.PeerId]);
            var clientpeer = PeerFactory.CreatePeer<IClientPeer>(new PeerConfig());
            clientpeer.PeerId = clientPeerGuid;
            
            // Add to connection collection
            ConnectionCollection.Connect(clientpeer);

            Response response;
            if (playerData != null)
            {
                var player = new Player()
                {
                    UserId = playerData.UserId,
                    ServerPeer = peer,
                    Name = playerData.SelectedCharacter.Name,
                    ClientPeerId = clientPeerGuid
                };

                Log.DebugFormat("On Client EnterWorld:    New player added to world server {0}", player.Name);

                using (IRedisClient redis = ClientsManager.GetClient())
                {
                    Log.DebugFormat("The redis client is working here on world server");
                    //TO DO
                }

                var returnCode = WorldService.AddNewPlayerToWorld(player);

                if (returnCode == ReturnCode.WorldAddedNewPlayer)
                {
                    response = new Response(Code, SubCode, new Dictionary<byte, object>() { { (byte)MessageParameterCode.SubCodeParameterCode, SubCode }, { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] } }, "New player on world", (short)returnCode);
                }
                else
                {
                    response = new Response(Code, SubCode, new Dictionary<byte, object>() { { (byte)MessageParameterCode.SubCodeParameterCode, SubCode }, { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] } }, "Player is already in world", (short)returnCode);
                }
            }
            else
            {
                response = new Response(Code, SubCode, new Dictionary<byte, object>() { { (byte)MessageParameterCode.SubCodeParameterCode, SubCode }, { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] } }, "Invalid operation", (short)ReturnCode.OperationInvalid);
            }

            peer.SendMessage(response);

            return true;
        }
    }
}
