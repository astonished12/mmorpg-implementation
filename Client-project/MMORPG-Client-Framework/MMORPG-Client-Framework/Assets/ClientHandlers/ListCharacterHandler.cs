using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.MGFClient.Implementation;
using Assets.MGFClient.Interfaces;
using Assets.MGFClient.Message.Implementation;
using GameCommon;
using UnityEngine;

namespace Assets.ClientHandlers
{
    public class ListCharacterHandler : GameMessageHandler
    {
        protected override  void OnHandleMessage(Dictionary<byte, object> parameters, string debugMessage, int returnCode)
        {
            if (returnCode == (short)ReturnCode.Ok)
            {
                Debug.LogFormat("Character list - {0}", parameters[(byte)MessageParameterCode.Object]);
            }
            else
            {
                //Show the error
                //ShowError(response.DebugMessage);
                Debug.LogFormat("{0}", debugMessage);
            }
        }
    }

}
