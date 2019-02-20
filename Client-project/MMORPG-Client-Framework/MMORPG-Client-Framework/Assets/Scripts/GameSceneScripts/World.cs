﻿using UnityEngine;

namespace GameSceneScripts
{
    public class World : MonoBehaviour
    {
        /*

        private Player LocalPlayer { get; set; }
        private float _step = 5f;

        private Vector3 OldPosition { get; set; }
        private const float SendRate = 0.05f;
        private float _lastSendTime = 0;
        // Use this for initialization
        void Start()
        {
            PhotonEngine.Instance.WorldEnterOperation();
        }

        // Update is called once per frame
        void Update()
        {
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
                var p = PhotonServer.Instance.Players.FirstOrDefault(
                    n => n.CharacterName.Equals(PhotonServer.Instance.CharacterName));

                if (p != null)
                {
                    LocalPlayer = p;
                    p.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                    PhotonServer.Instance.ListPlayersOperation();
                }
                return;
            }

            TrySend();
        }

        private void MoveLogic()
        {
            for (int i = 0; i < PhotonServer.Instance.Players.Count; i++)
            {
                var player = PhotonServer.Instance.Players[i];
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
            if (LocalPlayer.Position != OldPosition && _lastSendTime < Time.time)
            {
                OldPosition = LocalPlayer.Position;
                Vector3 oldRotation = LocalPlayer.transform.eulerAngles;

                _lastSendTime = Time.time + SendRate;

                PhotonServer.Instance.MoveOperation(OldPosition.x, OldPosition.y, OldPosition.z, oldRotation.x, oldRotation.y, oldRotation.z);
                Animator tmp = LocalPlayer.GetComponent<Animator>();
                float speed = tmp.GetFloat("Speed");
                bool jump = tmp.GetBool("Jumping");
                bool die = tmp.GetBool("Die");
                bool respawn = tmp.GetBool("Respawn");
                bool attack = tmp.GetBool("Attack");
                //Debug.Log("PRTTEY DEBUG " + speed + " " + jump + " " + die + " " + respawn + " " + attack);
                PhotonServer.Instance.AnimatorOperations(speed, jump, die, respawn, attack);

            }
        }*/
    }
}