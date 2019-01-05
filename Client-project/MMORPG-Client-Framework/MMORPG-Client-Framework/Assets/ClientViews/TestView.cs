using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class TestView : MonoBehaviour
{
    public InputField CreateUserName;
    public InputField CreatePassword;
    public InputField LoginUserName;
    public InputField LoginPassword;


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

    public void SendLoginRequest()
    {
        OperationRequest request = new OperationRequest()
        {
            OperationCode = (byte)MessageOperationCode.Login,
            Parameters = new Dictionary<byte, object>()
                {{(byte) PhotonEngine.Instance.SubCodeParameterCode, MessageSubCode.LoginUserPass}}
        };

        request.Parameters.Add((byte)MessageParameterCode.LoginName, LoginUserName.text);
        request.Parameters.Add((byte)MessageParameterCode.Password, LoginPassword.text);

        Debug.Log("Sending request for Login");
        PhotonEngine.Instance.SendRequest(request);
    }

    public void SendNewAccountRequest()
    {
        OperationRequest request = new OperationRequest()
        {
            OperationCode = (byte)MessageOperationCode.Login,
            Parameters = new Dictionary<byte, object>()
                {{(byte) PhotonEngine.Instance.SubCodeParameterCode, MessageSubCode.LoginNewAccount}}
        };

        request.Parameters.Add((byte)MessageParameterCode.LoginName, CreateUserName.text);
        request.Parameters.Add((byte)MessageParameterCode.Password, CreatePassword.text);
        Debug.Log("Sending request for Register");
        PhotonEngine.Instance.SendRequest(request);
    }
}
