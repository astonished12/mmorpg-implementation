using System.Collections.Generic;
using GameCommon;
using Debug = UnityEngine.Debug;

namespace ClientHandlers.Authentication
{
    public class AccountCreationHandler: GameMessageHandler
    {
        protected override void OnHandleMessage(Dictionary<byte, object> parameters, string debugMessage, int returnCode)
        {
            if (returnCode == (short) ReturnCode.Ok)
            {
                //Show the login server
                Debug.LogFormat("Account Created Successfully");
            }
            else
            {
                //Show the error
                //ShowError(response.DebugMessage);
                Debug.LogFormat("{0}",debugMessage);
            }
        }
    }
}
