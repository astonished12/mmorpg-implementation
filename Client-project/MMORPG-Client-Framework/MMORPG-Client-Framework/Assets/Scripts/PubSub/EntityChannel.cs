using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ServiceStack.Redis;
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
                    subscription.OnMessage = (channel, msg) => { };
                }

                subscription.SubscribeToChannels($"Entity_{Name}");
            }
        );
        thread.Start();

    }
}