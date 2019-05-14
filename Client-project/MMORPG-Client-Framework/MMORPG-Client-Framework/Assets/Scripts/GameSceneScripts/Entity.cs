using System.Collections;
using System.Collections.Generic;
using Photon.MmoDemo.Common;
using UnityEngine;

public class Entity : MonoBehaviour {
	protected Transform transform { get; set; }

	public string EntityName { get; set; }
	
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

	public Vector startPosition { get; set; }
	public float speed { set; get; }
	public bool die { get; set; }
	public bool respawn { get; set; }
	public bool attack { get; set; }

	void Awake()
	{
		transform = base.transform;
		NewPosition = Position;
	}
}
