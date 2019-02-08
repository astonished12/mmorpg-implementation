using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterSceneScripts
{
    public class CharacterSlot : MonoBehaviour
    {
        public Image image;
        public Text characterName;
        public Text characterLevel;

        void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => GetComponentsInParent<CharacterSlotController>()[0].CharacterSlotSelected(this));
        }
    }
}
