using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;

namespace Servers.Handlers.Regions
{
    public class ClientEnterRegion: IHandler<IServerPeer>
    {

        public MessageType Type
        {
            get { return MessageType.Request; }
        }

        public byte Code
        {
            get { return (byte) MessageOperationCode.Region; }
        }
        public int? SubCode
        {
            get
            {
                return  (int?) MessageSubCode.EnterRegion;
            }
        }

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            return true;
        }
    }
}
