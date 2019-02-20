using Servers.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Servers.Models
{
    public class WorldCache : IWorldCache, IDisposable
    {
        public static readonly WorldCache Instance = new WorldCache();
        private readonly Dictionary<string, World> dict;
        private readonly ReaderWriterLockSlim readWriteLock;


        public WorldCache()
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
