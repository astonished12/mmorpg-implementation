using System;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.MGFClient.Implementation;
using Assets.MGFClient.Interfaces;
using Assets.MGFClient.Message.Implementation;
using GameCommon;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.ClientHandlers
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
