using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGF.Domain;
using Servers.Services.Interfaces;
using ServiceStack.Redis;

namespace Servers.Services.CacheService
{
    public class CacheService: ICacheService
    {
        private IRedisClientsManager ClientsManager { get; set; }

        public CacheService(IRedisClientsManager clientsManager)
        {
            ClientsManager = clientsManager;
        }

        public Servers.Models.Character GetCharacterByName(string name)
        {
            using (IRedisClient redis = ClientsManager.GetClient())
            {
                return redis.Get<Servers.Models.Character>(name);
            }

        }
    }
}
