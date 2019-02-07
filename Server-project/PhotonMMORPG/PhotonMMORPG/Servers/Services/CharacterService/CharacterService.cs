using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;
using MGF.Domain;
using MGF.Mappers;
using Servers.Interfaces;

namespace Servers.Services.CharacterService
{
    public class CharacterService: ICharacterService
    {
        public ReturnCode CreateNewCharacter(int userId, string characterName, string characterClass)
        {
            if (String.IsNullOrWhiteSpace(characterName) || String.IsNullOrWhiteSpace(characterClass))
            {
                return ReturnCode.InvalidCharacterAndClass;
            }

            CharacterMapper characterMapper = new CharacterMapper();
            Character character = CharacterMapper.LoadByName(characterName);

            if (character != null)
            {
                return ReturnCode.DuplicateCharacterName;
            }

            character = new Character()
            {   
                UserId = userId,
                Name = characterName,
                Class = characterClass,
                Level = 0,
                ExperiencePoints = (decimal)0
            };

            characterMapper.Save(character);

            return ReturnCode.Ok;
        }

    }
}
