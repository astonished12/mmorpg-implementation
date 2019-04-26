using System.Collections;
using System.Collections.Generic;
using ClientHandlers;
using GameCommon;
using MGFClient;
using UnityEngine;

public class PlayerEnterRegionHandler : GameMessageHandler {
	protected override void OnHandleMessage(Dictionary<byte, object> parameters, string debugMessage, int returnCode)
	{
		Debug.LogFormat("Here I receive data from server player's object's and my character. Need to instantiate");
		var selectedCharacter =
			MessageSerializerService.DeserializeObjectOfType<GameCommon.SerializedObjects.Character>(
				parameters[(byte) MessageParameterCode.Object]);
	
		if (selectedCharacter != null)
		{
			GameData.Instance.selectedCharacter = selectedCharacter;
			
			var characterName = selectedCharacter.Name;
			var posX = selectedCharacter.Loc_X;
			var posZ = selectedCharacter.Loc_Y;
			var posY = Terrain.activeTerrain.terrainData.GetHeight((int)posX, (int)posZ);

			Debug.Log(posX + " " + posY + " " + posZ);
			Vector3 characterPosition = new Vector3(posX, posY, posZ);
			characterPosition.y = Terrain.activeTerrain.SampleHeight(characterPosition);

			var obj = Instantiate(Resources.Load("Hammer Warrior")) as GameObject;
			obj.transform.position = new Vector3(posX, posY, posZ);

			var player = obj.AddComponent<Player>();
			player.CharacterName = characterName;

			GameData.Instance.players.Add(player);

			Debug.Log("WorldEnterHandler charName:" + characterName);
		}
	}
}
