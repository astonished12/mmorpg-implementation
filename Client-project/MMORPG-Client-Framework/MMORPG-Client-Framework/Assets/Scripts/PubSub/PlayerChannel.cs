using System.Threading;
using System.Threading.Tasks;
using ServiceStack.Redis;
using UnityEngine;

namespace PubSub
{
	public class PlayerChannel
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
				{
					using (var subscription = ClientSub.CreateSubscription())
					{
						subscription.OnSubscribe = channel => { Debug.Log($"Client Subscribed to '{channel}'"); };
						subscription.OnUnSubscribe = channel => { Debug.Log($"Client #{channel} UnSubscribed from "); };
						subscription.OnMessage = (channel, msg) =>
						{
							Debug.Log($"Client  Received '{msg}' from channel '{channel}'");
							this.SendNotification("get object message");
						};
						subscription.SubscribeToChannels($"Client_{Name}");

					}
				}
				;
			});
			thread.Start();
		}


		public void SendNotification(string message)
		{
			Debug.Log("Get object info");
			ClientPub.PublishMessage($"Server_{Name}", message);
		}


	}
}
