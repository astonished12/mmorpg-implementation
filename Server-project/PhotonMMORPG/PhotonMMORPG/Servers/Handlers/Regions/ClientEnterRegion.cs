using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using GameCommon;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using Omu.ValueInjecter;
using Servers.Data.Client;
using Servers.Models;
using Servers.Services.Interfaces;
using ServiceStack.Redis;

namespace Servers.Handlers.Regions
{
    public class ClientEnterRegion : IHandler<IServerPeer>
    {

        private ILogger Log { get; set; }
        private IRegionService RegionService { get; set; }
        private ICacheService CacheService { get; set; }

        public ClientEnterRegion(ILogger log, IRegionService regionService, ICacheService cacheService)
        {
            Log = log;
            RegionService = regionService;
            CacheService = cacheService;
        }

        public MessageType Type => MessageType.Request;

        public byte Code => (byte) MessageOperationCode.World;
        public int? SubCode => (int?) MessageSubCode.EnterRegion;

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            var playerData =
                MessageSerializerService.DeserializeObjectOfType<CharacterData>(
                    message.Parameters[(byte) MessageParameterCode.Object]);
            var clientPeerGuid = new Guid((byte[])message.Parameters[(byte)MessageParameterCode.PeerId]);

            Response response;
            if (playerData != null)
            {
                var player = new Player()
                {
                    UserId = playerData.UserId,
                    ServerPeer = peer,
                    Name = playerData.SelectedCharacter.Name,
                    ClientPeerId = clientPeerGuid,
                    Character = CacheService.GetCharacterByName(playerData.SelectedCharacter.Name)
                 };

                var returnCode  = RegionService.AddPlayer(player);
                if (returnCode == ReturnCode.RegionAddedNewPlayer)
                {
                    // in service
                    response = new Response(Code, SubCode, new Dictionary<byte, object>() { { (byte)MessageParameterCode.SubCodeParameterCode, SubCode },
                        { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] } ,
                        { (byte)MessageParameterCode.Object,
                            MessageSerializerService.SerializeObjectOfType((GameCommon.SerializedObjects.Character)new GameCommon.SerializedObjects.Character().InjectFrom(player.Character.CharacterDataFromDb))
                        }
                    }, "New player on region", (short)returnCode);
                }
                else
                {
                    player = RegionService.GetPlayer(playerData.SelectedCharacter.Name) as Player;
                    // in service
                    response = new Response(Code, SubCode, new Dictionary<byte, object>()
                    {
                        { (byte)MessageParameterCode.SubCodeParameterCode, SubCode },
                        { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] },
                        { (byte)MessageParameterCode.Object,
                            MessageSerializerService.SerializeObjectOfType((GameCommon.SerializedObjects.Character)new GameCommon.SerializedObjects.Character().InjectFrom(player.Character.CharacterDataFromDb))
                        }
                    }, "Player is already in region", (short)returnCode);
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
