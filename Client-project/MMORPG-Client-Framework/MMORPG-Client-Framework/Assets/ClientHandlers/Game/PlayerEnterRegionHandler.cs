using System.Collections;
using System.Collections.Generic;
using ClientHandlers;
using UnityEngine;

public class PlayerEnterRegionHandler : GameMessageHandler {
	protected override void OnHandleMessage(Dictionary<byte, object> parameters, string debugMessage, int returnCode)
	{
		Debug.LogFormat("Here I receive data from server player's object's and my character. Need to instantiate");
	}
}
