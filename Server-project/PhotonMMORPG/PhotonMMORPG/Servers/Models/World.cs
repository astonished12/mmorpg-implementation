using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExitGames.Threading;
using GameCommon;
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

        public ReturnCode AddPlayer(IPlayer player)
        {
            using (ReadLock.TryEnter(this.readerWriterLock, 1000))
            {
                var newPlayer = Clients.FirstOrDefault(pl => pl.UserId == player.UserId);
                ReturnCode returnCode;
                if (null == newPlayer)
                {
                    Clients.Add(player);
                    returnCode = ReturnCode.WorldAddedNewPlayer;
                }
                else
                {
                    returnCode = ReturnCode.WorldContainsPlayer;
                }

                return returnCode;
            }
        }

        public ReturnCode RemovePlayer(IPlayer player)
        {
            Clients.Remove(player);
            return ReturnCode.Ok;
        }
       
    }
}
