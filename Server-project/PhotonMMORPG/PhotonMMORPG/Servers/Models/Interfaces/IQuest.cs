using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers.Models.Interfaces
{
    public interface IQuest
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}
