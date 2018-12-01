using Assets.MGFClient.Implementation;

namespace Assets.MGFClient.Interfaces
{
    public interface IMessageHandler
    {
        MessageType Type { get; }
        byte Code { get; }
        int? SubCode { get; }
        bool HandleMessage(IMessage message);
    }
}
