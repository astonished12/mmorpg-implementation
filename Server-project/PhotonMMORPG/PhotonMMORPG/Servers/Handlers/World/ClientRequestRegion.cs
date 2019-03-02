using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using GameCommon;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using Servers.Data.Client;
using Servers.Models;
using Servers.Services.Interfaces;

namespace Servers.Handlers.World
{
    public class ClientRequestRegion : IHandler<IServerPeer>
    {
        private ILogger Log { get; set; }
        private IWorldService WorldService { get; set; }
        private IConnectionCollection<IClientPeer> ConnectionCollection { get; set; }

        public ClientRequestRegion(ILogger log, IWorldService worldService, IConnectionCollection<IClientPeer> connectionCollection)
        {
            Log = log;
            WorldService = worldService;
            ConnectionCollection = connectionCollection;
        }

        public MessageType Type => MessageType.Request;

        public byte Code => (byte)MessageOperationCode.World;

        public int? SubCode => (int?)MessageSubCode.RequestRegion;

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            var playerData = MessageSerializerService.DeserializeObjectOfType<CharacterData>(message.Parameters[(byte)MessageParameterCode.Object]);
            Log.DebugFormat("OnClientRequestRegion: Client {0} request region", playerData.SelectedCharacter.Name);

            var player = new Player()
            {
                UserId = playerData.UserId,
                ServerPeer = peer,
                World = WorldService.GetWorld(),
                CharacterName = playerData.SelectedCharacter.Name,
                Client = ConnectionCollection.GetPeers<IClientPeer>().FirstOrDefault(x => x.PeerId == new Guid((byte[])message.Parameters[(byte)MessageParameterCode.PeerId]))
            };

            WorldService.GetRegionForPlayer(player);

            return true;
        }
    }
}
