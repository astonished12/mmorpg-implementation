using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servers.Models.Interfaces;

namespace Servers.Models
{
    public class Quest : IQuest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
