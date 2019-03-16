using System.Collections;
using System.Collections.Generic;
using ClientHandlers;
using GameCommon;
using LoadingSceneScripts;
using MGFClient;
using UnityEngine;

namespace ClientHandlers.Loading
{
    public class PlayerWorldRequestRegionResponseHandler : GameMessageHandler
    {
        protected override void OnHandleMessage(Dictionary<byte, object> parameters, string debugMessage,
            int returnCode)
        {
            if (returnCode == (short) ReturnCode.Ok)
            {
                var region =
                    MessageSerializerService.DeserializeObjectOfType<GameCommon.SerializedObjects.Region>(
                        parameters[(byte) MessageParameterCode.Object]);

                GameData.Instance.region = region;
                LoadingScene.imageComp.fillAmount += 0.25f;

                Debug.LogFormat("Region of character is selected successfully. Your region is {0}", region.Name);
            }
        }
    }
}