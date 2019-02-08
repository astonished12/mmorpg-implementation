using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using GameCommon;
using UnityEngine;

namespace ClientHandlers.Character
{
    public class ListCharacterResponseHandler : GameMessageHandler
    {
        public CharacterSlotController controller;

        protected override  void OnHandleMessage(Dictionary<byte, object> parameters, string debugMessage, int returnCode)
        {
            if (returnCode == (short)ReturnCode.Ok)
            {
                var listChar =
                    MessageSerializerService.DeserializeObjectOfType<IEnumerable<GameCommon.SerializedObjects.Character>>(parameters[(byte) MessageParameterCode.Object]);
                
                controller.LoadCharacterListInScene(listChar.ToList());
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
