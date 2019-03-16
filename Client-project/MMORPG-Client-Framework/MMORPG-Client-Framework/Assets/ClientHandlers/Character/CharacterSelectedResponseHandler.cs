using System.Collections.Generic;
using GameCommon;
using MGFClient;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClientHandlers.Character
{
    public class CharacterSelectedResponseHandler : GameMessageHandler
    {
        protected override void OnHandleMessage(Dictionary<byte, object> parameters, string debugMessage, int returnCode)
        {
            if (returnCode == (short) ReturnCode.Ok)
            {
                var selectedCharacter =
                    MessageSerializerService.DeserializeObjectOfType<GameCommon.SerializedObjects.Character>(
                        parameters[(byte) MessageParameterCode.Object]);
                
                Debug.LogFormat("Character selected successfully. Your character is {0}", selectedCharacter.Name);
                GameData.Instance.selectedCharacter = selectedCharacter;
                SceneManager.LoadScene(3); // go back to select character scene
            }
            else
            {
                Debug.LogFormat("{0}", debugMessage);
            }
        }
    }
}