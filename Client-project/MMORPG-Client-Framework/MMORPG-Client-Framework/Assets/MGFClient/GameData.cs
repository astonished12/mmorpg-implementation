using System;
using System.Collections.Generic;
using GameCommon.SerializedObjects;
using UnityEngine;

namespace MGFClient
{
	[Serializable]
	public class GameData : MonoBehaviour
	{
	
		public static GameData Instance = null;
	
		[SerializeField]
		public Character selectedCharacter;
		
		[SerializeField]
		public List<Character> characters;

		[SerializeField]
		public Region region;
	
		void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else if (Instance != this)
			{
				//Destroy object duplicate
				Destroy(gameObject);
			}

			DontDestroyOnLoad(gameObject);
		}
		

	}
}
