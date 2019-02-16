using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon.SerializedObjects;
using MultiplayerGameFramework.Interfaces.Client;

namespace Servers.Data.Client
{
    public class CharacterData : IClientData
    {
        public int UserId { get; set; }
        public List<Character> Characters { get; set; }
        public Character SelectedCharacter { get; set; }
        public Guid PeerId { get; set; }
    }
}
