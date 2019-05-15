using Servers.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;

namespace Servers.Models
{
    public class Item : IObject
    {
        public IObject Target => throw new NotImplementedException();

        public Guid GlobalId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int InstanceId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsVisible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Vector Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IAreaRegion AreaRegion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ObjectType Type => throw new NotImplementedException();

        public string Prefab { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public float WidthRadius => throw new NotImplementedException();

        public float HeightRadius => throw new NotImplementedException();

        public List<IObject> TargetedBy => throw new NotImplementedException();

        public List<IObject> VisibleObjects => throw new NotImplementedException();
    }
}
