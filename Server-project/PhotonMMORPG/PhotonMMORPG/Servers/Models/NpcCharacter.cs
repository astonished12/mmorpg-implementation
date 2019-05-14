using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.MmoDemo.Common;
using Servers.Models.Interfaces;
using Servers.Models.Templates;

namespace Servers.Models
{
    public class NpcCharacter : ICharacter
    {
        public NpcTemplate NpcTemplate { get; set; }
        public Vector Position { get; set; }
        public Vector StartPosition { get; set; }
    }
}
