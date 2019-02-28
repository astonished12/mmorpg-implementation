using System.Collections;
using System.Collections.Generic;
using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour {

	 private RectTransform rectComponent;
        public static Image imageComp;
    
        public float speed = 200f;
        public Text text;
        public Text textNormal;
        public int progress = 0;

        void Awake()
        {
            SendWorldEnter();
        }
        // Use this for initialization
        void Start () {
            rectComponent = GetComponent<RectTransform>();
            imageComp = transform.GetComponent<Image>();
        }
    	
    	// Update is called once per frame
    	void Update () {
            if (imageComp.fillAmount >= 0.5f && imageComp.fillAmount != 1f)
            {
                imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;
            }

            if (imageComp.fillAmount != 1f)
            {
                progress = (int)(imageComp.fillAmount * 100);
                if (progress > 0 && progress <= 33)
                {
                    textNormal.text = "Loading...";
                }
                else if (progress > 33 && progress <= 67)
                {
                    textNormal.text = "Wait ...";
                }
                else if (progress > 67 && progress <= 100)
                {
                    textNormal.text = "Please wait...";
                }
                else {
    
                }
                text.text = progress + "%";
            }
            else
            {
                imageComp.fillAmount = 0.0f;
                text.text = "0%";
            }
        }
        
        public void SendWorldEnter(){
            Debug.Log("Send message to world enter");
            PhotonEngine.Instance.SendRequest(MessageOperationCode.World, MessageSubCode.EnterWorld,null);
        }
}
