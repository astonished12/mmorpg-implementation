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
                        var npcCharactersAoi = msg.FromJson<List<NpcCharacter>>();
                        GameData.Instance.npcCharacters = npcCharactersAoi;
                        foreach (var npcCharacter in npcCharactersAoi)
                        {
                            if (GameData.Instance.npcCharacters.FirstOrDefault(x =>
                                x.StartPosition.Equals(npcCharacter.StartPosition)) != null)
                            {
                                continue;
                            }
                            
                            GameData.Instance.npcCharacters.Add(npcCharacter);
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