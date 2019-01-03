using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GameCommon;
using UnityEngine;

public class TestView : MonoBehaviour
{
    public void SendResponseRequest()
    {
        Debug.Log("Send for response");
        OperationRequest request = new OperationRequest()
        {
            OperationCode = 1,
            Parameters = new Dictionary<byte, object>()
                {{(byte) PhotonEngine.Instance.SubCodeParameterCode, 1}}
        };
        PhotonEngine.Instance.SendRequest(request);
    }

    public void SendEventRequest()
    {
        Debug.Log("Send for event");

        OperationRequest request = new OperationRequest()
        {
            OperationCode = (byte)MessageOperationCode.Login,
            Parameters = new Dictionary<byte, object>()
                {{(byte) PhotonEngine.Instance.SubCodeParameterCode, 2}}
        };
        PhotonEngine.Instance.SendRequest(request);

    }
}
