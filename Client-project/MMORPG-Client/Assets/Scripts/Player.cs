using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    protected Transform transform { get; set; }

    public string CharacterName { get; set; }

    public CharacterController controller { get; set; }
    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Quaternion Rotation
    {
        get { return transform.rotation; }
        set { transform.rotation = value; }
    }



    public Vector3 NewPosition { set; get; }
    public Quaternion NewRotation { set; get; }

    public float speed { set; get; }
    public bool jump { get; set; }
    public bool die { get; set; }
    public bool respawn { get; set; }
    public bool attack { get; set; }

    void Awake()
    {
        transform = base.transform;
        NewPosition = Position;
        controller = GetComponent<CharacterController>();
    }

}
