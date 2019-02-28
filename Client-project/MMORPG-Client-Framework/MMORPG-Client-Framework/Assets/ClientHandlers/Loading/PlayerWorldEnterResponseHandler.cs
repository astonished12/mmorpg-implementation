using System.Collections.Generic;
using ClientHandlers;
using GameCommon;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerWorldEnterResponseHandler: GameMessageHandler
    {
        protected override void OnHandleMessage(Dictionary<byte, object> parameters, string debugMessage, int returnCode)
        {
            LoadingScene.imageComp.fillAmount = 0.25f;
            Debug.Log("Client ");
        }
        
        public void SendGetRegion(){
            Debug.Log("Send message : request region");
            PhotonEngine.Instance.SendRequest(MessageOperationCode.World, MessageSubCode.RequestRegion,null);
        }
    }
}