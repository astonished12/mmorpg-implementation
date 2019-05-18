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
        public string Name { get; set; }
        private RedisClient ClientSub { get; set; }
        private RedisClient ClientPub { get; set; }


        public PlayerChannel(string name)
        {
            ClientSub = new RedisClient("localhost: 6379");
            ClientPub = new RedisClient("localhost: 6379");
            Name = name;

            Thread thread = new Thread(delegate()
            {
                //Here is a new thread
                IRedisSubscription subscription = null;

                using (subscription = ClientSub.CreateSubscription())
                {
                    subscription.OnSubscribe = channel => { Debug.Log($"Client Subscribed to '{channel}'"); };
                    subscription.OnUnSubscribe = channel => { Debug.Log($"Client #{channel} UnSubscribed from "); };
                    subscription.OnMessage = (channel, msg) =>
                    {
                        var entitiesAoi = msg.FromJson<List<ICharacter>>();
                        var npcCharactersAoi = entitiesAoi.OfType<NpcCharacter>();
                        var charactersAoi = entitiesAoi.OfType<Character>();
                        foreach (var npcCharacter in npcCharactersAoi)
                        {
                            if (GameData.Instance.npcCharacters.FirstOrDefault(x =>
                                    x.NpcTemplate.StartPosition.Equals(npcCharacter.NpcTemplate.StartPosition)) == null)
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

                        //this.SendNotification("get object message");
                    };
                }

                subscription.SubscribeToChannels($"Client_{Name}");
            });
            thread.Start();
        }


        public void SendNotification(string message)
        {
            ClientPub.PublishMessage($"Server_{Name}", message);
        }
    }
}