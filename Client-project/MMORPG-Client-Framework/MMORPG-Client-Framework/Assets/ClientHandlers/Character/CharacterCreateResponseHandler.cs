using System.Collections.Generic;
using GameCommon;
using UnityEngine;

namespace ClientHandlers.Character
{
    public class CharacterCreateResponseHandler : GameMessageHandler
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
                Debug.LogFormat("{0}", debugMessage);
            }
        }
    }
}