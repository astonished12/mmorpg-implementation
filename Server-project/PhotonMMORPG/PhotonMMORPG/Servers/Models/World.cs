using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExitGames.Threading;
using Servers.Models.Interfaces;

namespace Servers.Models
{
    public class World : IWorld
    {
        public static readonly World Instance = new World();
        public List<IPlayer> Clients { get; set; }
        private readonly ReaderWriterLockSlim readerWriterLock;

        public int WorldTick { get; }

        public World()
        {
            Clients = new List<IPlayer>();
            readerWriterLock = new ReaderWriterLockSlim();
        }

        public IRegion GetRegion(Guid id)
        {
            throw new NotImplementedException();
        }

        public void AddPlayer(IPlayer player)
        {
            using (ReadLock.TryEnter(this.readerWriterLock, 1000))
            {
                Clients.Add(player);
            }
        }

        public void RemovePlayer(IPlayer player)
        {
            Clients.Remove(player);
        }
       
    }
}
