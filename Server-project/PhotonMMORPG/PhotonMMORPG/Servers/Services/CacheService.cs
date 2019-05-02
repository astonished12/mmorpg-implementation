using Servers.Models;
using Servers.Services.Interfaces;
using ServiceStack.Redis;

namespace Servers.Services
{
    public class CacheService : ICacheService
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

        public void AddOrUpdateCharacter(string playerName, Character character)
        {
            using (IRedisClient redis = ClientsManager.GetClient())
            {
                redis.Set(playerName, character);
            }
        }
    }
}
