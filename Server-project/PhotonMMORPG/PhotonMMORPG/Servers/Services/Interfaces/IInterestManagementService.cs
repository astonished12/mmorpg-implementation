using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Interfaces.Client;
using Servers.Models;
using Servers.Models.Interfaces;

namespace Servers.Services.Interfaces
{
    public interface IInterestManagementService
    {
        void ComputeAreaOfInterest(IClientPeer peer, IPlayer player);
        List<NpcCharacter> GetAreaOfInterest(IClientPeer peer);
    }
}
