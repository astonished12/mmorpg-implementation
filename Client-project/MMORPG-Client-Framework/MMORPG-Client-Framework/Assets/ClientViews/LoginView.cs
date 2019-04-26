using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GameCommon;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : MonoBehaviour {
   public InputField CreateUserName;
    public InputField CreatePassword;
    public InputField LoginUserName;
    public InputField LoginPassword;

    void Awake()
    {
        if (Environment.IsTestEnvironment())
        {
            LoginPassword.text = "test";
            LoginUserName.text = "test";
        }
    }
    public void SendLoginRequest()
    {
        OperationRequest request = new OperationRequest() { OperationCode = (byte)MessageOperationCode.Login, Parameters = new Dictionary<byte, object>() { { (byte)PhotonEngine.Instance.SubCodeParameterCode, MessageSubCode.LoginUserPass } } };
        request.Parameters.Add((byte)MessageParameterCode.LoginName, LoginUserName.text);
        request.Parameters.Add((byte)MessageParameterCode.Password, LoginPassword.text);
        Debug.Log("Sending Request for Login");
        PhotonEngine.Instance.SendRequest(request);

    }

    public void SendNewAccountRequest()
    {
        OperationRequest request = new OperationRequest() { OperationCode = (byte)MessageOperationCode.Login, Parameters = new Dictionary<byte, object>() { { (byte)PhotonEngine.Instance.SubCodeParameterCode, MessageSubCode.LoginNewAccount } } };
        request.Parameters.Add((byte)MessageParameterCode.LoginName, CreateUserName.text);
        request.Parameters.Add((byte)MessageParameterCode.Password, CreatePassword.text);
        Debug.Log("Sending Request for New Account");
        PhotonEngine.Instance.SendRequest(request);
    }

}
