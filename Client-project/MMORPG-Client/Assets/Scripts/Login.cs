using PhotonMMORPG.Common;
using PhotonMMORPG.Common.CustomEventArgs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {

    private string Error { get; set; }
    private string CharacterName { get; set; }
    // Use this for initialization
    void Start() {
        PhotonServer.Instance.OnLoginResponse += OnLoginHandler;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnGUI()
    {
        CharacterName = GUI.TextField(new Rect(50, 50, 200, 30), CharacterName);

        if (GUI.Button(new Rect(50, 80, 100, 30), "Login")) {
            Error = "";
            PhotonServer.Instance.SendLoginOperation(CharacterName);
        }

        GUI.Label(new Rect(50, 5, 300, 20), Error);
    }

    private void OnLoginHandler(object o, LoginEventArgs e)
    {

        if(e.Error != ErrorCode.Ok)
        {
            Error = "Error:" + e.Error.ToString();
            Debug.Log(Error);
            return;
        }
        PhotonServer.Instance.OnLoginResponse -= OnLoginHandler;
        PhotonServer.Instance.CharacterName = CharacterName;
        SceneManager.LoadScene(1); //GAME
        
    }
}
