using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using PhotonMMORPG.Common;

namespace PhotonMMORPG.Operations
{
    class ChatMessage : BaseOperation
    {
        public ChatMessage(IRpcProtocol protocol, OperationRequest request) : base(protocol, request)
        {
        }

        [DataMember(Code = (byte)ParameterCode.ChatMessage)]
        public string Message { get; set; }
    }
}
