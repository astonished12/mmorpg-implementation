using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.MGFClient.Implementation;
using Assets.MGFClient.Interfaces;

namespace Assets.MGFClient.Message.Implementation
{
    public class Response : IMessage
    {
        protected readonly byte _code;
        protected readonly int? _subCode;
        protected readonly Dictionary<byte, object> _parameters;

        protected readonly string _debugMessage;
        protected readonly short _returnCode;

        public MessageType Type
        {
            get { return MessageType.Response; }

        }

        public byte Code
        {
            get { return _code; }
        }

        public int? SubCode
        {
            get { return _subCode; }
        }

        public Dictionary<byte, object> Parameters
        {
            get { return _parameters; }
        }

        public string DebugMessage
        {
            get { return _debugMessage; }
        }

        public short ReturnCode
        {
            get { return _returnCode; }
        }

        public Response(byte code, int? subCode, Dictionary<byte, object> parameters)
        {
            _code = code;
            _subCode = subCode;
            _parameters = parameters;
        }

        public Response(byte code, int? subCode, Dictionary<byte, object> parameters, string debugMessage,
            short returnCode) : this(code, subCode, parameters)
        {
            _debugMessage = debugMessage;
            _returnCode = returnCode;
        }
    }
}