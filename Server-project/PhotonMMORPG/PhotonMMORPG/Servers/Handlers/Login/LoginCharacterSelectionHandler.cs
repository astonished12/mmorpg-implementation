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

namespace Servers.Handlers.Login
{
    class LoginCharacterSelectionHandler : IHandler<IServerPeer>
    {
        private IConnectionCollection<IClientPeer> ConnectionCollection;
        private ILogger Log { get; }

        public LoginCharacterSelectionHandler(IConnectionCollection<IClientPeer> connectionCollection, ILogger log)
        {
            Log = log;
            ConnectionCollection = connectionCollection;
        }

        public MessageType Type
        {
            get { return MessageType.Request; }
        }
        public byte Code
        {
            get { return (byte)MessageOperationCode.Login; }
        }
        public int? SubCode
        {
            get { return (int?)MessageSubCode.SelectCharacter; }
        }
        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            var clientPeerGuid = new Guid((byte[])message.Parameters[(byte) MessageParameterCode.PeerId]);
            Log.DebugFormat("On LoginCharacterSelectionHandler the client peer is {0}", clientPeerGuid);
           
            var clientData = ConnectionCollection.GetPeers<IClientPeer>().FirstOrDefault(x => x.PeerId == clientPeerGuid).ClientData<CharacterData>();

            var selectedCharacter = clientData.Characters.FirstOrDefault(character => character.Name == (string) message.Parameters[(byte) MessageParameterCode.CharacterName]);
            Log.DebugFormat("The selected character is {0}",selectedCharacter.Name);

            clientData.SelectedCharacter = selectedCharacter;
            clientData.PeerId = clientPeerGuid;

            Response response = clientData.Characters.Count == 0
                ? new Response(Code, SubCode, new Dictionary<byte, object>() { { (byte)MessageParameterCode.SubCodeParameterCode, SubCode }, { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] } }, "You don't have a character ", (short)ReturnCode.NoExistingCharacter)
                : new Response(Code, SubCode, new Dictionary<byte, object>() { { (byte)MessageParameterCode.SubCodeParameterCode, SubCode }, { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] }, {(byte)MessageParameterCode.Object, MessageSerializerService.SerializeObjectOfType(selectedCharacter)} }, "You selected character", (short)ReturnCode.Ok);

            peer.SendMessage(response);

            return true;
        }
    }
}
