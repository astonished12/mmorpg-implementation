using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ExitGames.Logging;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Support;

namespace Servers.BackgroundThreads.Region
{
    public class CheatPreventionThread : IBackgroundThread
    {
        private bool _isRunning;
        public ILogger Log { get; set; }
        public IConnectionCollection<IClientPeer> ConnectionCollection { get; set; }

        public CheatPreventionThread(IConnectionCollection<IClientPeer> connectionCollection, ILogger log) // Include IoC objects this ChannelThread needs i.e : IAreaRegion, IStats etc
        {
            ConnectionCollection = connectionCollection;
            Log = log;
        }

        public void Setup(IServerApplication server)
        {
            //DO nothing in this setup. Normall used for setting up before ChannelThread start
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

                    if (timer.Elapsed < TimeSpan.FromMilliseconds(5000)) //run every 5s
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
                    Log.ErrorFormat(string.Format("Exception occured in TestBackGroundThread - {0}", e.StackTrace));
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
            if (instance != null)
            {
                Log.DebugFormat("Sendig text message to peer");
                instance.SendMessage(new Event(1,2,new Dictionary<byte, object>()));
            }
        }
    }
}
