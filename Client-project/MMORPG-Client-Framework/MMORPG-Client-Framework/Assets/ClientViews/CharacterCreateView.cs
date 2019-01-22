using System.Collections;
using System.Collections.Generic;
using GameCommon;
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
        PhotonEngine.Instance.SendRequest(MessageOperationCode.Login, MessageSubCode.CreateCharacter, MessageParameterCode.CharacterName, characterName.text, MessageParameterCode.CharacterClass, characterClass.text);
    }

    public void SelectCharacterClass(string className)
    {
        characterClass.text = className;
    }
}
