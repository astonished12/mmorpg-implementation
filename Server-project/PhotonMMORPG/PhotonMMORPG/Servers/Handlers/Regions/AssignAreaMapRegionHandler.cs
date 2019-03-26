using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using GameCommon;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using Servers.Models;
using Servers.Services.Interfaces;

namespace Servers.Handlers.Regions
{
    public class AssignAreaMapRegionHandler : IHandler<IServerPeer>
    {
        private ILogger Log { get; set; }
        private IRegionService RegionService { get; set; }

        public AssignAreaMapRegionHandler(ILogger log, IRegionService regionService)
        {
            Log = log;
            RegionService = regionService;
        }

        public MessageType Type => MessageType.Request;

        public byte Code => (byte)MessageOperationCode.Region;

        public int? SubCode => (int) MessageSubCode.AssignAreaMap;

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            var regions = MessageSerializerService.DeserializeObjectOfType<Region[]>(message.Parameters[(byte) MessageParameterCode.Object]);

            return true;
        }
    }
}
