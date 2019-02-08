using System.Collections;
using System.Collections.Generic;
using CharacterSceneScripts;
using ExitGames.Client.Photon;
using GameCommon;
using GameCommon.SerializedObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CharacterSlotController : MonoBehaviour
    {
        public GameObject characterSlot;
        public string selectedCharacterName;
        public Button playButton;

        public void LoadCharacterListInScene(List<Character> characters)
        {
            foreach (var character in characters)
            {
                var newCharacterSlot = Instantiate(characterSlot, GameObject.Find("ScrollView").transform.Find("Viewport").GetChild(0).transform, false);
                newCharacterSlot.GetComponent<CharacterSlot>().characterName.text = character.Name;
                newCharacterSlot.GetComponent<CharacterSlot>().characterLevel.text = character.Level.ToString();
            }
        }
        
        public void SelectCharacter()
        {
            OperationRequest request = new OperationRequest()
            {
                OperationCode = (byte)MessageOperationCode.Login,
                Parameters = new Dictionary<byte, object>()
                {
                    {(byte) PhotonEngine.Instance.SubCodeParameterCode, MessageSubCode.SelectCharacter},
                    {(byte) (MessageParameterCode.CharacterName), selectedCharacterName}
                }
            };
            Debug.Log("Sending request for play with character selected");
            PhotonEngine.Instance.SendRequest(request);
        }

        public void CharacterSlotSelected(CharacterSlot slot)
        {
            selectedCharacterName = slot.characterName.text;
            playButton.interactable = true;
        }
    }

}
