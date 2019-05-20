using System;
using GameCommon;
using UnityEngine;

namespace GameSceneScripts
{
	public class Entity : MonoBehaviour {
		protected Transform transform { get; set; }

		public string EntityName { get; set; }
		public Guid Identifier { get; set; }
		public bool mustBeDestroyed { get; set; }
		
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

		public Vector3Net StartPosition { get; set; }
		
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
}
