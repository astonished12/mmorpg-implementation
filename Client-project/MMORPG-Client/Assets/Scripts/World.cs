using System.Linq;
using UnityEngine;
using System.Collections;

public class World : MonoBehaviour
{

    private Player localPlayer { get; set; }
    private float Step = 5f;

    private Vector3 oldPosition { get; set; }
    private const float SendRate = 0.05f;
    private float lastSendTime = 0;
    // Use this for initialization
    void Start()
    {
        PhotonServer.Instance.WorldEnterOperation();
    }

    // Update is called once per frame
    void Update()
    {
        if(localPlayer)
            localPlayer.GetComponent<Movement>().Move();

        try
        {
            MoveLogic(); //other clients
        }
        catch { }


    }

    void FixedUpdate()
    {
        if (localPlayer == null)
        {
            var p = PhotonServer.Instance.Players.FirstOrDefault(
                n => n.CharacterName.Equals(PhotonServer.Instance.CharacterName));

            if (p != null)
            {
                localPlayer = p;
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
            if (player != localPlayer)
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
        if (localPlayer.Position != oldPosition && lastSendTime < Time.time)
        {
            oldPosition = localPlayer.Position;
            Vector3 oldRotation = localPlayer.transform.eulerAngles;

            lastSendTime = Time.time + SendRate;

            PhotonServer.Instance.MoveOperation(oldPosition.x, oldPosition.y, oldPosition.z, oldRotation.x, oldRotation.y, oldRotation.z);
            Animator tmp = localPlayer.GetComponent<Animator>();
            float speed = tmp.GetFloat("Speed");
            bool jump = tmp.GetBool("Jumping");
            bool die = tmp.GetBool("Die");
            bool respawn = tmp.GetBool("Respawn");
            bool attack = tmp.GetBool("Attack");
            //Debug.Log("PRTTEY DEBUG " + speed + " " + jump + " " + die + " " + respawn + " " + attack);
            PhotonServer.Instance.AnimatorOperations(speed, jump, die, respawn, attack);

        }
    }
}
