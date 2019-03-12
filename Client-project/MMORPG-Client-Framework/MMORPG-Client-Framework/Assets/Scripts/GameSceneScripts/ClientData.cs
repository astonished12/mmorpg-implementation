using System.Collections;
using System.Collections.Generic;
using GameCommon.SerializedObjects;
using Photon.MmoDemo.Common;
using UnityEngine;

public class ClientData : MonoBehaviour
{
	
	public static ClientData ClientDataInstance = null;

	public Character character;
	public Region region;
	
	void Awake()
	{
		if (ClientDataInstance == null)
		{
			ClientDataInstance = this;
		}
		else if (ClientDataInstance != this)
		{
			//Destroy object duplicate
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}
	

}
