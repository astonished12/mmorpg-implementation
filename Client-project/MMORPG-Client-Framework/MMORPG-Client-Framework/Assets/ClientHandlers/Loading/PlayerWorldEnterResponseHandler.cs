using System.Collections.Generic;
using ClientHandlers;
using GameCommon;
using LoadingSceneScripts;
using MGFClient;
using PubSub;
using UnityEngine;

namespace ClientHandlers.Loading
{
    public class PlayerWorldEnterResponseHandler: GameMessageHandler
    {
        protected override void OnHandleMessage(Dictionary<byte, object> parameters, string debugMessage, int returnCode)
        {
            LoadingScene.imageComp.fillAmount += 0.25f;
            Debug.Log("Client was added to the world");
            GameData.Instance.channel = new PlayerChannel(parameters[(byte) MessageParameterCode.PlayerChannel].ToString());
            SendGetRegion();
        }
        
        
        public void SendGetRegion(){
            Debug.Log("Send message : request region");
            PhotonEngine.Instance.SendRequest(MessageOperationCode.World, MessageSubCode.RequestRegion,null);
        }
    }
}