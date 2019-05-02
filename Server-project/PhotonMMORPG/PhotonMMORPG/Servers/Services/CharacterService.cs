using System;
using System.Numerics;
using GameCommon;
using MGF.Domain;
using MGF.Mappers;
using Servers.Services.Interfaces;

namespace Servers.Services
{
    public class CharacterService : ICharacterService
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

            var spawnPoint = new Vector3(10f, 0f, 5f);
            character = new Character()
            {
                UserId = userId,
                Name = characterName,
                Class = characterClass,
                Level = 1,
                Loc_X = spawnPoint.X,
                Loc_Y = spawnPoint.Y,
                Loc_Z = spawnPoint.Z,
                ExperiencePoints = (decimal)0
            };

            characterMapper.Save(character);

            return ReturnCode.Ok;
        }
    }
}
