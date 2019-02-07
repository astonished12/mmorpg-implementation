using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;

namespace Servers.Interfaces
{
    public interface ICharacterService
    {
        ReturnCode CreateNewCharacter(int userId, string characterName, string characterClass);
    }
}
