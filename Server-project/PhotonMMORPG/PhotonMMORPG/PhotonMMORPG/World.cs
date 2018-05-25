using ExitGames.Logging;
using ExitGames.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PhotonMMORPG
{
    public class World
    {
        public static readonly World Instance = new World(); //singleton => all customers

        public List<UnityClient> Clients { get; set; }
           
        private readonly ReaderWriterLockSlim readerWriterLock;

        private readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public World()
        {
            Clients = new List<UnityClient>();
            readerWriterLock = new ReaderWriterLockSlim();
        }

        ~World()
        {
            readerWriterLock.Dispose();
        }

        public bool IsContain(string name)
        {
            using(ReadLock.TryEnter(this.readerWriterLock, 1000))
            {
                return Clients.Exists(n => n.CharacterName.Equals(name));
            }
        }

        public void AddClient(UnityClient client)
        {
            using (ReadLock.TryEnter(this.readerWriterLock, 1000))
            {

                Clients.Add(client);
            }
        }

        public void RemoveClient(UnityClient client)
        {
            Clients.Remove(client);
        }

        public void ViewCurrentClients()
        {
            foreach(UnityClient client in Clients) {
                Log.Debug("Client: " + client.CharacterName+" is active");
            }
        }

        public UnityClient TryGetByName(string name)
        {
            using(ReadLock.TryEnter(this.readerWriterLock, 1000))
            {
                return Clients.Find(n => n.CharacterName.Equals(name));
            }
        }

    }

}
