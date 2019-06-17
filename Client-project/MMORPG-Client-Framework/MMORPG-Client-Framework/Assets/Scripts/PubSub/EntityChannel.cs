using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GameCommon.SerializedObjects;
using MGFClient;
using Newtonsoft.Json;
using ServiceStack;
using ServiceStack.Redis;
using StackExchange.Redis;
using UnityEngine;

public class EntityChannel : MonoBehaviour
{
    public ConnectionMultiplexer Client { get; set; }

    public string Name { get; set; }
    public Thread thread;
    public EntityChannel(string name)
    {
        Client =  ConnectionMultiplexer.Connect("127.0.0.1");
        Name = name;

        thread = new Thread(delegate()
            {
                //Here is a new thread
                var subscription = Client.GetSubscriber();
                subscription.Subscribe($"Entity_{Name}", (channel, msg) =>
                {

                    Debug.Log("New message from channel " + channel);
                    var entitiesAoi = JsonConvert.DeserializeObject<Character>(msg);
                    var player =
                        GameData.Instance.players.FirstOrDefault(x => x.CharacterName.Equals(entitiesAoi.Name));

                    player.NewPosition = new Vector3(entitiesAoi.Loc_X, entitiesAoi.Loc_Y, entitiesAoi.Loc_Z);
                    Debug.Log(player.NewPosition);
                    player.NewRotation = Quaternion.Euler(entitiesAoi.Rot_X, entitiesAoi.Rot_Y, entitiesAoi.Rot_Z);
                    player.die = entitiesAoi.Die;
                    player.jump = entitiesAoi.Jump;
                    player.attack = entitiesAoi.Attack;
                    player.speed = entitiesAoi.Speed;
                    player.respawn = entitiesAoi.Respawn;
                });
            }
        );
        thread.Start();

    }
}