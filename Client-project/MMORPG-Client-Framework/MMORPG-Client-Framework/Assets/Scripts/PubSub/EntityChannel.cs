using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GameCommon.SerializedObjects;
using MGFClient;
using ServiceStack.Redis;
using ServiceStack.Text;
using UnityEngine;

public class EntityChannel : MonoBehaviour
{
    private RedisClient ClientPubSub { get; set; }
    public string Name { get; set; }
    public Thread thread;
    public EntityChannel(string name)
    {
        ClientPubSub = new RedisClient("localhost: 6379");
        Name = name;

        thread = new Thread(delegate()
            {
                //Here is a new thread
                IRedisSubscription subscription = null;
                using (subscription = ClientPubSub.CreateSubscription())
                {
                    subscription.OnSubscribe = channel => { Debug.Log($"Client Subscribed to '{channel}'"); };
                    subscription.OnUnSubscribe = channel => { Debug.Log($"Client #{channel} UnSubscribed from "); };
                    subscription.OnMessage = (channel, msg) =>
                    {
                        var entitiesAoi = msg.FromJson<Character>();
                        var player = GameData.Instance.players.FirstOrDefault(x => x.CharacterName.Equals(entitiesAoi.Name));
                        player.NewPosition = new Vector3(entitiesAoi.Loc_X, entitiesAoi.Loc_Y, entitiesAoi.Loc_Z);
                    };
                }

                subscription.SubscribeToChannels($"Entity_{Name}");
            }
        );
        thread.Start();

    }
}