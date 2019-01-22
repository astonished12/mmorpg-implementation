using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GameCommon;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CharacterSlotController : MonoBehaviour
    {
        public GameObject characterSlot;
        public int selectedCharacterId;
        public Button playButton;

        public void SelectCharacter()
        {
            OperationRequest request = new OperationRequest()
            {
                OperationCode = (byte)MessageOperationCode.Login,
                Parameters = new Dictionary<byte, object>()
                {
                    {(byte) PhotonEngine.Instance.SubCodeParameterCode, MessageSubCode.SelectCharacter},
                    {(byte) (MessageParameterCode.Object), selectedCharacterId}
                }
            };
            Debug.Log("Sending request for character list");
            PhotonEngine.Instance.SendRequest(request);
        }

        public void CharacterSlotSelected(CharacterSlot slot)
        {
            selectedCharacterId = slot.characterId;
            playButton.interactable = true;
        }
    }

}
