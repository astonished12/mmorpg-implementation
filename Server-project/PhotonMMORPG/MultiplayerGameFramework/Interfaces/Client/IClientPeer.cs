using MultiplayerGameFramework.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Interfaces.Client
{
    public interface IClientPeer // how client talk to server
    {
        bool IsProxy { get; set; } 
        Guid PeerId { get; set; }
        T ClientData<T>() where T : class, IClientData; // store anything with IClientData ( we can take whatever)
        void Disconnect();
        void SendMessage(IMessage message); // send messege from server to which client we want
    }
}
