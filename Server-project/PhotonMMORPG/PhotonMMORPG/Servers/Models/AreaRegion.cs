using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using GameCommon;
using MultiplayerGameFramework.Interfaces.Server;
using Photon.SocketServer.Concurrency;
using Servers.Models.Factories;
using Servers.Models.Interfaces;
using Servers.Models.Templates;
using StackExchange.Redis;

namespace Servers.Models
{

    public class AreaRegion : IAreaRegion
    {
        public List<AreaRegionEntityDetail> Entities;
        private List<NpcCharacter> _npcCharactersFromTemplate;
        public MapTemplate MapTemplate;
        
        private readonly NpcFactory _npcFactory = new NpcFactory();

        public AreaRegion(int x, int y, MapTemplate mapTemplate)
        {
            this.X = x;
            this.Y = y;
            this.MapTemplate = mapTemplate;
        }

        public void AssignCharactersFromNpcTemplate()
        {
            _npcCharactersFromTemplate = _npcFactory.Create(this.MapTemplate.Spawns);
            Entities = new List<AreaRegionEntityDetail>();
            foreach (var mapTemplateSpawn in MapTemplate.Spawns)
            {
                Entities.Add(new AreaRegionEntityDetail { Position = mapTemplateSpawn, NpcCharacters = new List<NpcCharacter>() });
            }
        }

        // grid cell X (debug only)
        public int X { get; private set; }

        // grid cell Y (debug only)
        public int Y { get; private set; }

        public string Name { get; set; }

        public Guid ZoneId { get; set; }

        public IWorld World { get; set; }

        public int GameTick { get; set; }

        public string ApplicationServerName { get; set; }

        public void AddNpcCharacter(Vector pos, NpcCharacter obj)
        {
            var spawnZone = Entities.FirstOrDefault(x => x.Position.Equals(pos));
            spawnZone?.NpcCharacters.Add(obj);
        }

        public void RemoveNpcCharacter(Vector pos, NpcCharacter obj)
        {
            var spawnZone = Entities.FirstOrDefault(x => x.Position.Equals(pos));
            spawnZone?.NpcCharacters.Remove(obj);
        }


        public override string ToString()
        {
            return string.Format("AreaAreaRegion({0},{1})", base.ToString(), X, Y);
        }

        public void SpawnMobs(Vector spawnPosition)
        {
            var npcCharacter = _npcCharactersFromTemplate.FirstOrDefault(x => x.Position.Equals(spawnPosition));
            if (npcCharacter != null) AddNpcCharacter(spawnPosition, npcCharacter);
        }
    }
}
