using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servers.Models.Interfaces;

namespace Servers.Models
{
    public class Character: ICharacter
    {
        public MGF.Domain.Character CharacterDataFromDb { get; set; }
    }
}
