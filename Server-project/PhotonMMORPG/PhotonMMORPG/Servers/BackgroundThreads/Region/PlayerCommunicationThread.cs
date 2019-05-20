using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExitGames.Logging;
using GameCommon;
using GameCommon.SerializedObjects;
using MultiplayerGameFramework.Interfaces;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Support;
using Servers.Models.Interfaces;
using Servers.Services.Interfaces;
using ItemDrop = GameCommon.SerializedObjects.ItemDrop;

namespace Servers.BackgroundThreads.Region
{
    public class PlayerCommunicationThread : IBackgroundThread
    {
        private bool _isRunning;
        private ILogger Log { get; set; }
        private IConnectionCollection<IClientPeer> ConnectionCollection { get; set; }
        private IInterestManagementService InterestManagementService { get; set; }
        private IRegion Region { get; set; }
        private ICacheService CacheService { get; set; }
        private IRegionService RegionService { get; set; }

        public PlayerCommunicationThread(IConnectionCollection<IClientPeer> connectionCollection, ILogger log,
            IInterestManagementService interestManagementService, ICacheService cacheService,
            IRegionService regionService, IRegion region) // Include IoC objects this thread needs i.e : IAreaRegion, IStats etc
        {
            ConnectionCollection = connectionCollection;
            Log = log;
            InterestManagementService = interestManagementService;
            CacheService = cacheService;
            Region = region;
            RegionService = regionService;
        }

        public void Setup(IServerApplication server)
        {
            //DO nothing in this setup. Normall used for setting up before thread start
        }

        public void Run(object threadContext)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            _isRunning = true;

            while (_isRunning)
            {
                try
                {
                    //check to see if there are any players => We need a  of players. If we have no players sleep => protect cpu
                    if (ConnectionCollection.GetPeers<IClientPeer>().Count <= 0)
                    {
                        Thread.Sleep(1000);
                        timer.Restart();
                    }

                    if (timer.Elapsed < TimeSpan.FromMilliseconds(1000)) //run every 5s
                    {
                        if (1000 - timer.ElapsedMilliseconds > 0) // no cpu fries:))
                        {
                            Thread.Sleep(1000 - (int)timer.ElapsedMilliseconds);
                        }
                        continue;
                    }

                    Update(timer.Elapsed);
                    //Restart the timer so that, just in case it takes longer than 100ms
                    timer.Restart();
                }
                catch (Exception e)
                {
                    Log.ErrorFormat($"Exception occured in AreaOfInterestThread - {e.StackTrace}");
                }
            }
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Update(TimeSpan elapsed)
        {
            foreach (var client in Region.ClientsInRegion)
            {
                client.Character = CacheService.GetCharacterByName(client.Character.CharacterDataFromDb.Name);
            }

            Parallel.ForEach(ConnectionCollection.GetPeers<IClientPeer>(), SendUpdate);
        }

        public void SendUpdate(IClientPeer peer)
        {
            var regionClient = Region.ClientsInRegion.FirstOrDefault(x => x.ClientPeerId == peer.PeerId);
            if (peer != null && regionClient != null)
            {
                var playerChannel = RegionService.GetPlayerChannel(regionClient.Character.CharacterDataFromDb.Name);
                if (null == playerChannel)
                {
                    return;
                }
                
                var entitiesAoi = InterestManagementService.GetAreaOfInterest(peer)?.ToList();
                var entitiesAoiCommon = new List<GameCommon.SerializedObjects.ICharacter>();

                if (entitiesAoi != null && entitiesAoi.Count > 0)
                {
                    foreach (var entity in entitiesAoi)
                    {
                        if (entity is Servers.Models.NpcCharacter npcCharacter)
                        {
                            entitiesAoiCommon.Add(new GameCommon.SerializedObjects.NpcCharacter
                            {
                                NpcTemplate = new NpcTemplate
                                {
                                    Name = npcCharacter.NpcTemplate.Name,
                                    Stats = new Dictionary<Stat, float>(), // vezi mai incolo e grav:))
                                    Id = npcCharacter.NpcTemplate.Id,
                                    Type = npcCharacter.NpcTemplate.Type,
                                    Respawn = npcCharacter.NpcTemplate.Respawn,
                                    AiType = npcCharacter.NpcTemplate.AiType,
                                    DropList = new List<ItemDrop>(), // same here
                                    Prefab = npcCharacter.NpcTemplate.Prefab,
                                    WidthRadius = npcCharacter.NpcTemplate.WidthRadius,
                                    Position = new Vector3Net(npcCharacter.Position.X, npcCharacter.Position.Y, npcCharacter.Position.Z),
                                    StartPosition = new Vector3Net(npcCharacter.StartPosition.X, npcCharacter.StartPosition.Y, npcCharacter.StartPosition.Z),
                                    Rotation = new Vector3Net(npcCharacter.Rotation.X, npcCharacter.Rotation.Y, npcCharacter.Rotation.Z),
                                    StartRotation = new Vector3Net(npcCharacter.StartRotation.X, npcCharacter.StartRotation.Y, npcCharacter.StartRotation.Z),
                                    Identifier = npcCharacter.Identifier
                                }
                            });
                        }
                        else if (entity is Servers.Models.Character character)
                        {
                            entitiesAoiCommon.Add(new GameCommon.SerializedObjects.Character
                            {
                                Name = character.CharacterDataFromDb.Name,
                                Loc_X = character.CharacterDataFromDb.Loc_X,
                                Loc_Y = character.CharacterDataFromDb.Loc_Y,
                                Loc_Z = character.CharacterDataFromDb.Loc_Z,
                                ExperiencePoints = character.CharacterDataFromDb.ExperiencePoints,
                                Level = character.CharacterDataFromDb.Level,
                                Class = character.CharacterDataFromDb.Class,
                                LifePoints = character.CharacterDataFromDb.LifePoints,
                                ManaPoints = character.CharacterDataFromDb.ManaPoints

                            });
                        }

                    }
                }

                if (entitiesAoiCommon.Count > 0)
                {
                    playerChannel.SendInfoToMainClient(entitiesAoiCommon);
                }

            }


        }
    }
}
