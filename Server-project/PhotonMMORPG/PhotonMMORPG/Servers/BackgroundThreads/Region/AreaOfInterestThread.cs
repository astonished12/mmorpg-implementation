using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExitGames.Logging;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Support;
using Servers.Models.Interfaces;

namespace Servers.BackgroundThreads.Region
{
    public class AreaOfInterestThread : IBackgroundThread
    {
        private bool _isRunning;
        public ILogger Log { get; set; }
        public IConnectionCollection<IClientPeer> ConnectionCollection { get; set; }
        public IRegion Region { get; set; }

        public AreaOfInterestThread(IConnectionCollection<IClientPeer> connectionCollection, ILogger log, IRegion region) // Include IoC objects this thread needs i.e : IAreaRegion, IStats etc
        {
            ConnectionCollection = connectionCollection;
            Log = log;
            Region = region;
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
                        if (5000 - timer.ElapsedMilliseconds > 0) // no cpu fries:))
                        {
                            Thread.Sleep(5000 - (int)timer.ElapsedMilliseconds);
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
            Parallel.ForEach(ConnectionCollection.GetPeers<IClientPeer>(), SendUpdate);
        }

        public void SendUpdate(IClientPeer instance)
        {
            var regionClient = Region.ClientsInRegion.FirstOrDefault(x => x.ClientPeerId == instance.PeerId);
            if (instance != null && regionClient != null)
            {
                Log.DebugFormat($"Sendig text message to peer {instance.PeerId} and {regionClient.Name}");
                //instance.SendMessage(new Event(1,2,new Dictionary<byte, object>()));
            }
        }
    }
}
