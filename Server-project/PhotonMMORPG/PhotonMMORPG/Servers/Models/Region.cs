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
    public class Region : IRegion
    {
        public List<IPlayer> ClientsInRegion { get; set; }
        public AreaRegion[] AreaRegions { get; set; }
        private readonly ReaderWriterLockSlim _readerWriterLock;

        public Region()
        {
            ClientsInRegion = new List<IPlayer>();
            _readerWriterLock = new ReaderWriterLockSlim();

        }

        public ReturnCode AddPlayer(IPlayer player)
        {
            using (ReadLock.TryEnter(this._readerWriterLock, 1000))
            {
                var newPlayer = ClientsInRegion.FirstOrDefault(pl => pl.UserId == player.UserId);
                ReturnCode returnCode;
                if (null == newPlayer)
                {
                    ClientsInRegion.Add(player);
                    returnCode = ReturnCode.RegionAddedNewPlayer;
                }
                else
                {
                    returnCode = ReturnCode.RegionContainsPlayer;
                }

                return returnCode;
            }
        }

        public ReturnCode RemovePlayer(IPlayer player)
        {
            ClientsInRegion.Remove(player);
            return ReturnCode.Ok;
        }


    }
}
