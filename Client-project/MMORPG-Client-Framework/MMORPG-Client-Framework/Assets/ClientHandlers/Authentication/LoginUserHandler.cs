using System.Collections.Generic;
using GameCommon;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace ClientHandlers.Authentication
{
    public class LoginUserHandler : GameMessageHandler
    {
        protected override void OnHandleMessage(Dictionary<byte, object> parameters, string debugMessage, int returnCode)
        {
            if (returnCode == (short)ReturnCode.Ok)
            {
                // successful login
                Debug.Log("Succesfull login");
                SceneManager.LoadScene("CharacterSelect");
            }
            else
            {
                Debug.LogFormat("{0} - {1}", this.name, debugMessage);
            }
        }
    }
}
