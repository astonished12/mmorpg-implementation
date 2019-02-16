using System.Collections.Generic;
using GameCommon;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClientHandlers.Character
{
    public class CharacterCreateResponseHandler : GameMessageHandler
    {
        protected override void OnHandleMessage(Dictionary<byte, object> parameters, string debugMessage, int returnCode)
        {
            if (returnCode == (short) ReturnCode.Ok)
            {
                Debug.LogFormat("Account Created Successfully");
                SceneManager.LoadScene(1); // go back to select character scene
            }
            else
            {
                Debug.LogFormat("{0}", debugMessage);
            }
        }
    }
}