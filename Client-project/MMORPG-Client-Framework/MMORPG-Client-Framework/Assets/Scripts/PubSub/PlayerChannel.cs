using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameCommon;
using GameCommon.SerializedObjects;
using MGFClient;
using ServiceStack.Redis;
using ServiceStack.Text;
using UnityEngine;

namespace PubSub
{
    public class PlayerChannel : MonoBehaviour
    {
        private string Name { get; set; }
        private RedisClient ClientPubSub { get; set; }

        public PlayerChannel(string name)
        {
            ClientPubSub = new RedisClient("localhost: 6379");
            Name = name;

            var thread = new Thread(delegate()
            {
                //Here is a new thread
                IRedisSubscription subscription = null;
                using (subscription = ClientPubSub.CreateSubscription())
                {
                    subscription.OnSubscribe = channel => { Debug.Log($"Client Subscribed to '{channel}'"); };
                    subscription.OnUnSubscribe = channel => { Debug.Log($"Client #{channel} UnSubscribed from "); };
                    subscription.OnMessage = (channel, msg) =>
                    {
                        var entitiesAoi = msg.FromJson<List<ICharacter>>();
                        var npcCharactersAoi = entitiesAoi.OfType<NpcCharacter>().ToList();
                        var charactersAoi = entitiesAoi.OfType<Character>();
                        foreach (var npcCharacter in npcCharactersAoi)
                        {
                            if (GameData.Instance.npcCharacters.FirstOrDefault(x =>
                                    x.NpcTemplate.Identifier.ToString().Equals(npcCharacter.NpcTemplate.Identifier.ToString())) == null)
                            {
                                lock (GameData.Instance.npcCharacters)
                                {
                                    GameData.Instance.npcCharacters.Add(npcCharacter);
                                }
                            }
                        }

                        foreach (var character in charactersAoi)
                        {
                            if (GameData.Instance.playersCharacters.FirstOrDefault(x =>
                                    x.Name.Equals(character.Name)) == null)
                            {
                                lock (GameData.Instance.characters)
                                {
                                    GameData.Instance.playersCharacters.Add(character);
                                }
                            }
                        }

                        //TO DO UNSUBSCRIBE AND DESTROY OBJECT OR MARK THEM INVISIBLE RAU TARE TARE MERGE CHALLENGING
                        
//                        var entitiesThatMustBeDestroyed =
//                            GameData.Instance.npcCharacters.Except(npcCharactersAoi).ToList();
//                      
//                        foreach (var entity in entitiesThatMustBeDestroyed)
//                        {
//                            var entityInEntitiesList =
//                                GameData.Instance.entities.SingleOrDefault(x =>
//                                    x.EntityName.Equals(entity.NpcTemplate.Identifier.ToString()));
//                            if (entityInEntitiesList != null) entityInEntitiesList.mustBeDestroyed = true;
//
//                            var channelInChannelList =
//                                GameData.Instance.pubSubActorsEntities.SingleOrDefault(x =>
//                                    x.Name.Equals(entity.NpcTemplate.Identifier.ToString()));
//                            GameData.Instance.pubSubActorsEntities.Remove(channelInChannelList);
//
//                            if (channelInChannelList != null) channelInChannelList.thread.Abort();
//                        }

                    };
                }

                subscription.SubscribeToChannels($"Client_{Name}");
            });
            thread.Start();
        }
       
    }
}