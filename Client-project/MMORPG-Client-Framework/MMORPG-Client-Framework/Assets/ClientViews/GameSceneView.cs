using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GameCommon;
using UnityEngine;

public class GameSceneView : MonoBehaviour {

	void Awake()
	{
		SendRequestEnterRegion();
	}

	public void SendRequestEnterRegion()
	{
		OperationRequest request = new OperationRequest()
		{
			OperationCode = (byte)MessageOperationCode.World,
			Parameters = new Dictionary<byte, object>()
				{{(byte) PhotonEngine.Instance.SubCodeParameterCode, MessageSubCode.EnterRegion}}
		};
		
		Debug.Log("Sending request for enter in region");
		PhotonEngine.Instance.SendRequest(request);
	}
	
}
