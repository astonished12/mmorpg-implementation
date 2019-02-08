using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using GameCommon;
using GameCommon.SerializedObjects;
using MGF.Mappers;
using MultiplayerGameFramework.Interfaces.Server;
using Servers.Interfaces;
using Servers.Services.CharacterService;

namespace Servers.Handlers.Login
{
    public class LoginCharacterCreationHandler : IHandler<IServerPeer>
    {
        private ILogger Log { get; }
        private ICharacterService CharacterService { get; }

        public MessageType Type => MessageType.Request;

        public byte Code => (byte)MessageOperationCode.Login;

        public int? SubCode => (int?)MessageSubCode.CreateCharacter;

        public LoginCharacterCreationHandler(ILogger log, ICharacterService characterService)
        {
            Log = log;
            CharacterService = characterService;
        }

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            var userId = (int)message.Parameters[(byte)MessageParameterCode.UserId];
            Log.DebugFormat("On LoginCharacterCreationHandler received this {0} as user id", userId);

            var characterName = message.Parameters[(byte)MessageParameterCode.CharacterName];
            var characterClass = message.Parameters[(byte)MessageParameterCode.CharacterClass];

            ReturnCode returnCode = CharacterService.CreateNewCharacter(userId, (string)characterName, (string)characterClass);
            Response response;

            if (returnCode == ReturnCode.DuplicateCharacterName)
            {
                response = new Response(Code, SubCode, new Dictionary<byte, object>() { { (byte)MessageParameterCode.SubCodeParameterCode, SubCode }, { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] } }, "A character with that name exists", (short)ReturnCode.InvalidCharacterAndClass);
            }
            else
            {
                response = new Response(Code, SubCode, new Dictionary<byte, object>() { { (byte)MessageParameterCode.SubCodeParameterCode, SubCode }, { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] } }, "Character created", (short)ReturnCode.Ok);
            }

            peer.SendMessage(response);

            return true;
        }
    }
}
