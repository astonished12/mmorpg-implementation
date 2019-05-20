using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using GameCommon;
using MultiplayerGameFramework.Implementation.Client;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using Photon.SocketServer;
using Servers.Data.Client;
using Servers.Services.Interfaces;
using ServiceStack;

namespace Servers.Handlers.World
{
    public class ClientMove : IHandler<IServerPeer>
    {
        private ICacheService CacheService { get; set; }
        private IConnectionCollection<IClientPeer> ClientConnectionCollection { get; set; }
        private IWorldService WorldService { get; set; }
        private ILogger Log { get; set; }

        public MessageType Type => MessageType.Request;
        public byte Code => (byte)MessageOperationCode.World;
        public int? SubCode => (byte)MessageSubCode.Move;

        public ClientMove(ICacheService cacheService, IConnectionCollection<IClientPeer> clientConnectionCollection, IWorldService worldService, ILogger log)
        {
            CacheService = cacheService;
            ClientConnectionCollection = clientConnectionCollection;
            WorldService = worldService;
            Log = log;
        }

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            var playerData = MessageSerializerService.DeserializeObjectOfType<CharacterData>(message.Parameters[(byte)MessageParameterCode.Object]);
            var player = WorldService.GetPlayer(playerData.SelectedCharacter.Name);
            player.Character = CacheService.GetCharacterByName(playerData.SelectedCharacter.Name);
            
            var returnCode = WorldService.UpdatePositionAndRotation(player,
                message.Parameters[(byte)MessageParameterCode.PosX], message.Parameters[(byte)MessageParameterCode.PosY], message.Parameters[(byte)MessageParameterCode.PosZ],
                message.Parameters[(byte)MessageParameterCode.RotX], message.Parameters[(byte)MessageParameterCode.RotY], message.Parameters[(byte)MessageParameterCode.RotZ]);


            var response = new Response((byte)MessageOperationCode.World, (byte)MessageSubCode.Move, new Dictionary<byte, object> {
                {(byte) MessageParameterCode.Object, message.Parameters[(byte)MessageParameterCode.Object]},
                {(byte)MessageParameterCode.CharacterName, playerData.SelectedCharacter.Name }
            });

            if (returnCode == ReturnCode.Ok)
            {
                Log.DebugFormat("Client move registered for {0}", player.Character.CharacterDataFromDb.Name);
                peer.SendMessage(response);
            }
            else
            {
                Log.DebugFormat("Invalid parameters for move");
            }
            return true;
        }

    }
}
