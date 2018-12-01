using Assets.MGFClient.Implementation;
using Assets.MGFClient.Interfaces;

namespace Assets.ClientHandlers
{
    public class TestHandler : IMessageHandler
    {
        public MessageType Type
        {
            get { return MessageType.Async; }
        }

        public byte Code
        {
            get { return 1; }
        }
        public int? SubCode { get { return 1; } }
        public bool HandleMessage(IMessage message)
        {
            //Do nothing
            return true;
        }
    }
}
