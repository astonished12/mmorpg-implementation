using System.Collections.Generic;
using Assets.Scripts;
using GameCommon;
using UnityEngine;

namespace ClientHandlers.Character
{
    public class ListCharacterHandler : GameMessageHandler
    {
        public CharacterSlotController controller;

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
