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

namespace Servers.Handlers.Regions
{
    public class ClientEnterRegion: IHandler<IServerPeer>
    {

        private ILogger Log { get; set; }

        public ClientEnterRegion(ILogger log)
        {
            Log = log;
        }

        public MessageType Type => MessageType.Request;

        public byte Code => (byte) MessageOperationCode.Region;
        public int? SubCode => (int?) MessageSubCode.EnterRegion;

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            Log.DebugFormat("Here must be logged");   
            return true;
        }
    }
}
