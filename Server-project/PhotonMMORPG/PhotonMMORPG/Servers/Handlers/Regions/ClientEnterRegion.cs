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
using Servers.Data.Client;
using Servers.Services.Interfaces;

namespace Servers.Handlers.Regions
{
    public class ClientEnterRegion : IHandler<IServerPeer>
    {

        private ILogger Log { get; set; }
        private IRegionService RegionService { get; set; }

        public ClientEnterRegion(ILogger log, IRegionService regionService)
        {
            Log = log;
            RegionService = regionService;
        }

        public MessageType Type => MessageType.Request;

        public byte Code => (byte) MessageOperationCode.World;
        public int? SubCode => (int?) MessageSubCode.EnterRegion;

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            var clientData =
                MessageSerializerService.DeserializeObjectOfType<CharacterData>(
                    message.Parameters[(byte) MessageParameterCode.Object]);

            Log.DebugFormat("My mucu is here");

            Response response = new Response(Code, SubCode, new Dictionary<byte, object>()); 
            peer.SendMessage(response);
            return true;
        }
    }
}
