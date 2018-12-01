using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;

namespace Servers.Handlers
{
    public class TestRequestResponseHandler : IHandler<IClientPeer>
    {
        public MessageType Type => MessageType.Request;
        public byte Code => 1;
        public int? SubCode => 1;
        public bool HandleMessage(IMessage message, IClientPeer peer)
        {
            throw new NotImplementedException();
        }
    }
}
