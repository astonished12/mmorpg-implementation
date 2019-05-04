using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.MmoDemo.Common;
using Servers.Models.Interfaces;
using Servers.Models.Templates;

namespace Servers.Models.Factories
{
    public class AreaFactory : IFactory
    {
        public Dictionary<int, MapTemplate> MapTemplates { get; set; }

        public AreaFactory()
        {
            MapTemplates = new Dictionary<int, MapTemplate>();
            LoadTemplates();
        }

        private void LoadTemplates()
        {
            MapTemplates.Add(0, new MapTemplate
            {
                Name = "Area Region 0",
                Id = 0,
                StartPosition = new Vector(0f,0f,0f),
                SceneName = "GameScene",
                ZoneHeight = 250,
                ZoneLength = 250,
                Spawns = new List<Vector>()
                {
                    new Vector(76f,0f,16f),
                    new Vector(90f,0f,16f),
                    new Vector(147f,0f,16f),
                    new Vector(203f,0f,72f),
                    new Vector(216f,0f,121f),
                }
            });

            MapTemplates.Add(1, new MapTemplate
            {
                Name = "Area Region 1",
                Id = 0,
                StartPosition = new Vector(250f, 0f, 0f),
                SceneName = "GameScene",
                ZoneHeight = 250,
                ZoneLength = 250,
                Spawns = new List<Vector>()
            });

            MapTemplates.Add(2, new MapTemplate
            {
                Name = "Area Region 2",
                Id = 0,
                StartPosition = new Vector(0f, 0f, 250f),
                SceneName = "GameScene",
                ZoneHeight = 250,
                ZoneLength = 250,
                Spawns = new List<Vector>()
            });

            MapTemplates.Add(3, new MapTemplate
            {
                Name = "Area Region 4",
                Id = 0,
                StartPosition = new Vector(250f, 0f, 250f),
                SceneName = "GameScene",
                ZoneHeight = 250,
                ZoneLength = 250,
                Spawns = new List<Vector>()
            });
        }

        public AreaRegion[][] Create(int tileX, int tileY)
        {
            var worldAreaRegions = new AreaRegion[tileX][];
            var totalRegion = 0;
            var npcFactory = new NpcFactory();

            for (var x = 0; x < tileX; x++)
            {
                worldAreaRegions[x] = new AreaRegion[tileY];
                for (var y = 0; y < tileY; y++)
                {
                    worldAreaRegions[x][y] = new AreaRegion(x, y, this.MapTemplates[totalRegion])
                    {
                        Name = this.MapTemplates[totalRegion].Name,
                        ZoneId = Guid.NewGuid(),
                    };
                    totalRegion += 1;
                }
            }

            return worldAreaRegions;
        }
    }
}
