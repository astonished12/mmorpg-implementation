using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers.Services.Interfaces
{
    public interface ICacheService
    {
       Servers.Models.Character GetCharacterByName(string name);
    }
}
