using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;

namespace Servers.Models.Interfaces
{
    public interface IObject
    {
        IObject Target { get; }
        Guid GlobalId { get; set; }
        int InstanceId { get; set; }
        bool IsVisible { get; set; }
        string Name { get; set; }
        String Description { get; set; }
        Position Position { get; set; }
        IAreaRegion AreaRegion { get; set; }
        ObjectType Type { get; }

        string Prefab { get; set; }
        float WidthRadius { get; }
        float HeightRadius { get; }
        List<IObject> TargetedBy { get; }
        List<IObject> VisibleObjects { get; }
    }
}
