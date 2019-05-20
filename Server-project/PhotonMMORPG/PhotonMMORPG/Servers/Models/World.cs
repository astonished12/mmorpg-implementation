using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExitGames.Threading;
using GameCommon;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Server;
using Servers.Config;
using Servers.Models.Factories;
using Servers.Models.Interfaces;

namespace Servers.Models
{
    public class World : IWorld
    {
        public List<IPlayer> Clients { get; set; }
        public GridWorld GridWorld { get; }

        private readonly ReaderWriterLockSlim _readerWriterLock;
        private readonly BoundingBox _boundingBox;

        //Check how to get data from unity terrain
        private readonly int maxWidthTerrain = 500;
        private readonly int maxLengthTerrain = 500;
        // PUT THEM INTO CONFIG

        public World()
        {
            Clients = new List<IPlayer>();

            _readerWriterLock = new ReaderWriterLockSlim();
            _boundingBox = new BoundingBox(new Vector() { X = 0, Y = 0, Z = 0 }, 
                new Vector() { X = maxWidthTerrain, Y = maxLengthTerrain, Z = 0 });

            //4 region => the size of tile is the terrain width and length dive 2 
            GridWorld = new GridWorld(_boundingBox, new Vector(maxWidthTerrain / 2, maxLengthTerrain / 2));

            //ONE REGION JUST FOR TEST
            //GridWorld = new GridWorld(boundingBox, new Vector(maxWidthTerrain, maxLengthTerrain));

        }

        public IAreaRegion GetRegion(Vector pos)
        {
            return GridWorld.GetRegion(pos);
        }

        public ReturnCode AddPlayer(IPlayer player)
        {
            using (ReadLock.TryEnter(this._readerWriterLock, 1000))
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

        public IPlayer GetPlayer(string name)
        {
            using (ReadLock.TryEnter(this._readerWriterLock, 1000))
            {
                return Clients.FirstOrDefault(pl => pl.Name == name);
            }
        }

        public ReturnCode RemovePlayer(IPlayer player)
        {
            Clients.Remove(player);
            return ReturnCode.Ok;
        }

        public ReturnCode UpdatePlayerPositionAndRotation(IPlayer player, params object[] data)
        {
            var playerCharacter = Clients.FirstOrDefault(x => x.Name == player.Name)?.Character.CharacterDataFromDb;
            if (data.Length != 11)
            {
                return ReturnCode.OperationInvalid;
            }
          
            return ReturnCode.Ok;
        }
    }

}
