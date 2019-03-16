using System.Collections;
using System.Collections.Generic;
using GameCommon;
using MGFClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCreateView : MonoBehaviour
{
    public InputField characterName;
    public Text characterClass;

    public void ReturnToSelect()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public void SendCharacterCreateMessage()
    {
        Debug.Log("Sending for new character creation");
        if (characterClass.text.Equals("Select a combat class"))
        {
            Debug.Log("Select a combat class");
            //TO DO ALERT MESSAGE
            return;
        }
        PhotonEngine.Instance.SendRequest(MessageOperationCode.Login, MessageSubCode.CreateCharacter, MessageParameterCode.CharacterName, characterName.text, MessageParameterCode.CharacterClass, characterClass.text);
    }

    public void SelectCharacterClass(string className)
    {
        characterClass.text = className;
    }
}
