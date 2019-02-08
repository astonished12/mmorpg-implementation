using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using GameCommon;
using GameCommon.SerializedObjects;
using MGF.Mappers;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using MultiplayerGameFramework.Interfaces.Support;
using Servers.Data.Client;
using Servers.Interfaces;

namespace Servers.Handlers.Login
{
    public class LoginCharacterListCharacterHandler : IHandler<IServerPeer>
    {
        private ILogger Log { get; set; }

        private readonly IConnectionCollection<IClientPeer> ConnectionCollection;

        public MessageType Type => MessageType.Request;

        public byte Code => (byte)MessageOperationCode.Login;

        public int? SubCode => (int?)MessageSubCode.CharacterList;

        public LoginCharacterListCharacterHandler(ILogger log, IConnectionCollection<IClientPeer> connectionCollection)
        {
            Log = log;
            ConnectionCollection = connectionCollection;
        }

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            //how to prevent hacking? => Proxy server removed all special codes from the client and uses its own when forwarding data
            var userId = (int) message.Parameters[(byte) MessageParameterCode.UserId];
            Log.DebugFormat("Login Character Handler List received this {0} as user id", userId);

            var clientPeer = new Guid((byte[])message.Parameters[(byte)MessageParameterCode.PeerId]);

            var charList = new CharacterMapper().LoadByUserId(userId).Select(x => new Character
            {
                CharacterId = x.Id,
                Name = x.Name,
                Class = x.Class,
                Level = x.Level,
                ExperiencePoints = x.ExperiencePoints
            });

            //Added characterlist to clientData (remove 1 call to db)
            ConnectionCollection.GetPeers<IClientPeer>().FirstOrDefault(x => x.PeerId == clientPeer).ClientData<CharacterData>().Characters = charList.ToList();


            //We have a list of characters, added into a serializable object
            var returnResponse = new Response(Code, SubCode, new Dictionary<byte, object>()
            {
                { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId]},
                { (byte)MessageParameterCode.Object, MessageSerializerService.SerializeObjectOfType(charList)},
                { (byte)MessageParameterCode.SubCodeParameterCode, SubCode}
            });

            peer.SendMessage(returnResponse);
            return true;
        }
    }
}
