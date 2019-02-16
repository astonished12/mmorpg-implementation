using System.Collections;
using System.Collections.Generic;
using GameCommon;
using UnityEngine;

public class SceneView : MonoBehaviour {

	void Awake()
	{
		//TO DO MOVE IN LOADING SCENE 
		SendWorldEnter();
	}
	public void SendWorldEnter(){
		Debug.Log("Send message to world enter");
		PhotonEngine.Instance.SendRequest(MessageOperationCode.World, MessageSubCode.EnterWorld,null);
	}
}
