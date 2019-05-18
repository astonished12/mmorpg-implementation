using System.Collections.Generic;
using System.Linq;
using MGFClient;
using UnityEngine;

namespace GameSceneScripts
{
    public class World : MonoBehaviour
    {
        private Player LocalPlayer { get; set; }
        private float _step = 5f;

        public bool testMode;
        private Vector3 OldPosition { get; set; }
        private const float SendRate = 0.05f;
        private float _lastSendTime = 0;
        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
//            if (testMode)
//            {
//                GameObject.FindWithTag("Player").GetComponent<Movement>().Move();
//                GameObject.FindWithTag("Player").GetComponent<Movement>().transform.Find("Camera").gameObject.SetActive(true);
//                Camera.main.gameObject.SetActive(false);
//            }
//            
            if(LocalPlayer)
                LocalPlayer.GetComponent<Movement>().Move();

            try
            {
                MoveLogic(); //other clients
            }
            catch { }


        }

        void FixedUpdate()
        {
            if (LocalPlayer == null)
            {
                var p = GameData.Instance.players.FirstOrDefault(
                    n => n.CharacterName.Equals(GameData.Instance.selectedCharacter.Name));

                if (p != null)
                {
                    LocalPlayer = p;
                    p.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                }
                return;
            }

            TrySend();
        }

        private void MoveLogic()
        {
            for (int i = 0; i < GameData.Instance.players.Count; i++)
            {
                var player = GameData.Instance.players[i];
                if (player != LocalPlayer)
                {
                    player.Position = Vector3.Lerp(player.Position, player.NewPosition, Time.fixedDeltaTime * 15f);
                    player.Rotation = Quaternion.Slerp(player.Rotation, player.NewRotation, Time.fixedDeltaTime * 3f);
                    Animator tmp = player.GetComponent<Animator>();
                    tmp.SetFloat("Speed", player.speed);
                    tmp.SetBool("Jumping", player.jump);
                    tmp.SetBool("Die", player.die);
                    tmp.SetBool("Respawn", player.respawn);
                    tmp.SetBool("Attack", player.attack);
                }
            }
        }
      

        private void TrySend()
        {
            if (Vector3.SqrMagnitude(LocalPlayer.Position-OldPosition)>0.1f && _lastSendTime < Time.time)
            {
                OldPosition = LocalPlayer.Position;
                Vector3 oldRotation = LocalPlayer.transform.eulerAngles;

                _lastSendTime = Time.time + SendRate;

                GameObject.Find("GameView").GetComponent<GameSceneView>().SendMoveRquest(OldPosition, oldRotation);
                Animator tmp = LocalPlayer.GetComponent<Animator>();
                float speed = tmp.GetFloat("Speed");
                bool jump = tmp.GetBool("Jumping");
                bool die = tmp.GetBool("Die");
                bool respawn = tmp.GetBool("Respawn");
                bool attack = tmp.GetBool("Attack");
                //Debug.Log("PRTTEY DEBUG " + speed + " " + jump + " " + die + " " + respawn + " " + attack);
                //PhotonEngine.Instance.AnimatorOperations(speed, jump, die, respawn, attack);

            }
        }
    }
}
