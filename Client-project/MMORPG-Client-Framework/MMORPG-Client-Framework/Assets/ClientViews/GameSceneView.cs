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
		OperationRequest request = new OperationRequest
		{
			OperationCode = (byte)MessageOperationCode.World,
			Parameters = new Dictionary<byte, object> {{PhotonEngine.Instance.SubCodeParameterCode, MessageSubCode.EnterRegion}}
		};
		
		Debug.Log("Sending request for enter in region");
		PhotonEngine.Instance.SendRequest(request);
	}

	public void SendMoveRquest(Vector3 oldPosition, Vector3 oldRotation, float speed, bool jump, bool die,bool respawn, bool attack)
	{
		PhotonEngine.Instance.SendRequest(MessageOperationCode.World, MessageSubCode.Move, 
			MessageParameterCode.PosX,oldPosition.x, 
			MessageParameterCode.PosY, oldPosition.y, 
			MessageParameterCode.PosZ, oldPosition.z, 
			MessageParameterCode.RotX, oldRotation.x, 
			MessageParameterCode.RotY, oldRotation.y,
			MessageParameterCode.RotZ, oldRotation.z,
			MessageParameterCode.Speed, speed,
			MessageParameterCode.Jump, jump,
			MessageParameterCode.Die, die,
			MessageParameterCode.Respawn, respawn,
			MessageParameterCode.Attack, attack);
	}
	
}
