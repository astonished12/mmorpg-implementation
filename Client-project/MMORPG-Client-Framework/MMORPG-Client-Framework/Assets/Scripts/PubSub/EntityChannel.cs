using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GameCommon.SerializedObjects;
using MGFClient;
using ServiceStack;
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
                        Debug.Log("New message from channel "+channel);
                        var entitiesAoi = msg.FromJson<Character>();
                        var player = GameData.Instance.players.FirstOrDefault(x => x.CharacterName.Equals(entitiesAoi.Name));
                        player.NewPosition = new Vector3(entitiesAoi.Loc_X, entitiesAoi.Loc_Y, entitiesAoi.Loc_Z);
                        player.NewRotation = Quaternion.Euler(entitiesAoi.Rot_X, entitiesAoi.Rot_Y, entitiesAoi.Rot_Z );
                        player.die = entitiesAoi.Die;
                        player.jump = entitiesAoi.Jump;
                        player.attack = entitiesAoi.Attack;
                        player.speed = entitiesAoi.Speed;
                        player.respawn = entitiesAoi.Respawn;
                    };
                }

                subscription.SubscribeToChannels($"Entity_{Name}");
            }
        );
        thread.Start();

    }
}