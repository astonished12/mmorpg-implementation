using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using PhotonMMORPG.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer
{
    public class Login:BaseOperation
    {
        public Login(IRpcProtocol protocol, OperationRequest request):base(protocol, request)
        {

        }

        [DataMember(Code=(byte)ParameterCode.CharacterName)]
        public string CharacterName { get; set; }
    }
}
