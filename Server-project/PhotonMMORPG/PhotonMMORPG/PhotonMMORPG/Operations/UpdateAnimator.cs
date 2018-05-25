using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using PhotonMMORPG.Common;

namespace PhotonMMORPG.Operations
{
    class UpdateAnimator : BaseOperation
    {
        public UpdateAnimator(IRpcProtocol protocol, OperationRequest request) : base(protocol, request)
        {
        }

        [DataMember(Code = (byte)ParameterCode.Speed)]
        public float Speed { get; set; }

        [DataMember(Code = (byte)ParameterCode.Jump)]
        public bool Jump { get; set; }

        [DataMember(Code = (byte)ParameterCode.Die)]
        public bool Die { get; set; }


        [DataMember(Code = (byte)ParameterCode.Respawn)]
        public bool Respawn { get; set; }

        [DataMember(Code = (byte)ParameterCode.Attack)]
        public bool Attack { get; set; }

    }
}
