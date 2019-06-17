using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIData : MonoBehaviour {
	private Animator characterAnimator;

	private UnityEngine.AI.NavMeshAgent _navMeshAgentvMesh;
	private Camera camera;
	private List<Vector3> waypointsPositions;

	public GameObject waypoint1;
	public GameObject waypoint2;
	public GameObject waypoint3;
	private System.Random random = new System.Random();

	public bool isAI;

	void Awake()
	{
		characterAnimator = GetComponent<Animator>();
		_navMeshAgentvMesh = GetComponent<NavMeshAgent>();
		camera = this.transform.GetChild(2).gameObject.GetComponent<Camera>();

		waypoint1 = GameObject.Find("Waypoint1");
		waypoint2 = GameObject.Find("Waypoint2");
		waypoint3 = GameObject.Find("Waypoint3");

		waypointsPositions = new List<Vector3>()
		{
			waypoint1.transform.position,
			waypoint2.transform.position,
			waypoint3.transform.position
		};
		
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			isAI = !isAI;
			if (isAI == false)
			{
				_navMeshAgentvMesh.isStopped = true;
				characterAnimator.SetBool("AI", false);
			}
			else
			{
				_navMeshAgentvMesh.isStopped = false;
				MoveAI();
			}
		}

		if (CheckDestinationReached())
		{
			MoveAI();
		}
	}

	void MoveAI()
	{
		int index = random.Next(1, 4);
		_navMeshAgentvMesh.SetDestination(waypointsPositions[index - 1]);
		characterAnimator.SetBool("AI", true);
	}

	bool CheckDestinationReached()
	{
		if (_navMeshAgentvMesh.remainingDistance <= 3f)
		{
			characterAnimator.SetBool("AI", false);
			return true;
		}

		return false;
	}


}
