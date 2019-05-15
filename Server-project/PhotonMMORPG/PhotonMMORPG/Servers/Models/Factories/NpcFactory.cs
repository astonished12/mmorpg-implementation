using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Servers.Models.Interfaces;
using Servers.Models.Templates;
using Vector = GameCommon.Vector;

namespace Servers.Models.Factories
{
    public class NpcFactory : IFactory
    {
        public NpcFactory()
        {
            Templates = new Dictionary<int, NpcTemplate>();
            LoadTemplates();
        }

        public Dictionary<int, NpcTemplate> Templates { get; set; }


        public void LoadTemplates()
        {
            Templates.Add(0, new NpcTemplate
            {
                Id = 0,
                Name = "Skeleton",
                Prefab = "Skeleton",
                Type = RelationshipType.Aggressive,
                Respawn = 2000,
                AiType = null,
                DropList = new List<ItemDrop>
                {
                    new ItemDrop(0, 0.5f),
                    new ItemDrop(1, 1),
                    new ItemDrop(2, 1),
                    new ItemDrop(3, 1),
                    new ItemDrop(4, 1),
                },
                WidthRadius = 0.5f
            });
        }

        public List<NpcCharacter> Create(List<Vector> spawnPoints)
        {
            var npcCharacters = new List<NpcCharacter>();
            foreach (var spawnPoint in spawnPoints)
            {
                npcCharacters.Add(new NpcCharacter()
                {
                    NpcTemplate = Templates[0],
                    Position = spawnPoint,
                    StartPosition = spawnPoint
                });
            }

            return npcCharacters;
        }
    }
}
