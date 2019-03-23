using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Interfaces.Server;

namespace Servers.Models.Interfaces
{
    public interface IRegion
    {
        string Name { get; }
        Guid ZoneId { get; }
        IWorld World { get; }
        int GameTick { get; }
        string ApplicationServerName { get; }

        void AddObject(IObject obj);
        void RemoveObject(IObject obj);

    }
}
