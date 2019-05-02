using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servers.Models;

namespace Servers.Services.Interfaces
{
    public interface ICacheService
    {
       Servers.Models.Character GetCharacterByName(string name);
        void AddOrUpdateCharacter(string playerName, Character character);
    }
}
