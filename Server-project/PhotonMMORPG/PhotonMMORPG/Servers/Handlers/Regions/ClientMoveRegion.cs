using ExitGames.Logging;
using GameCommon;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using Servers.Data.Client;
using Servers.Models;
using Servers.Services.Interfaces;
using ServiceStack;

namespace Servers.Handlers.Regions
{
    public class ClientMoveRegion : IHandler<IServerPeer>
    {
        private ILogger Log { get; set; }
        private IRegionService RegionService { get; set; }

        public ClientMoveRegion(ILogger log, IRegionService regionService)
        {
            Log = log;
            RegionService = regionService;
        }

        public MessageType Type => MessageType.Request;

        public byte Code => (byte)MessageOperationCode.Region;

        public int? SubCode => (int)MessageSubCode.Move;

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            var character = MessageSerializerService.DeserializeObjectOfType<Character>(message.Parameters[(byte)MessageParameterCode.Object]);

            if (RegionService.GetPlayer(character.CharacterDataFromDb.Name) == null) return true;

            var playerChannel = RegionService.GetPlayerChannel(character.CharacterDataFromDb.Name);
            if (playerChannel.ChannelThread != null)
            {
                playerChannel.SendInfoToOthersClients(new GameCommon.SerializedObjects.Character
                {
                    Name = character.CharacterDataFromDb.Name,
                    Loc_X = character.CharacterDataFromDb.Loc_X,
                    Loc_Y = character.CharacterDataFromDb.Loc_Y,
                    Loc_Z = character.CharacterDataFromDb.Loc_Z,
                    ExperiencePoints = character.CharacterDataFromDb.ExperiencePoints,
                    Level = character.CharacterDataFromDb.Level,
                    Class = character.CharacterDataFromDb.Class,
                    LifePoints = character.CharacterDataFromDb.LifePoints,
                    ManaPoints = character.CharacterDataFromDb.ManaPoints
                });
            }
            return true;
        }
    }
}
