
using System.Collections.Generic;
using Assets.MGFClient.Implementation;

namespace Assets.MGFClient.Interfaces
{
    public interface IMessage
    {
        MessageType Type { get; }

        byte Code { get; }
        int? SubCode { get; }
        Dictionary<byte, object> Parameters { get; }
    }
}
