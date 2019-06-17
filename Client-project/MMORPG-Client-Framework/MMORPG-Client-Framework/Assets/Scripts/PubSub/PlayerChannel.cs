using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameCommon;
using GameCommon.SerializedObjects;
using MGFClient;
using Newtonsoft.Json;
using ServiceStack;
using ServiceStack.Redis;
using ServiceStack.Text;
using StackExchange.Redis;
using UnityEngine;

namespace PubSub
{
    public class PlayerChannel : MonoBehaviour
    {
        private string Name { get; set; }
        public ConnectionMultiplexer Client { get; set; }

        public PlayerChannel(string name)
        {
            Client =  ConnectionMultiplexer.Connect("127.0.0.1");
            Name = name;

            var thread = new Thread(delegate()
            {
                //Here is a new thread
                var subscription = Client.GetSubscriber();
                subscription.Subscribe($"Client_{Name}", (channel, msg) =>
                {
                    var entitiesAoi = JsonConvert.DeserializeObject<List<ICharacter>>(msg, new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                    
                    var npcCharactersAoi = entitiesAoi.OfType<NpcCharacter>().ToList();
                    var charactersAoi = entitiesAoi.OfType<Character>();
                    foreach (var npcCharacter in npcCharactersAoi)
                    {
                        if (GameData.Instance.npcCharacters.FirstOrDefault(x =>
                                x.NpcTemplate.Identifier.ToString()
                                    .Equals(npcCharacter.NpcTemplate.Identifier.ToString())) == null)
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

                });

            });
            thread.Start();
        }
       
    }
}