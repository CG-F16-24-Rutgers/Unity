using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour {
	public Transform[] waypoints;
	private NavMeshAgent agent;
	private int nextPoint;
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		nextPoint = 0;
		agent.destination = waypoints[0].position;
	}
	void Update () {
		Collider[] 
		if (agent.remainingDistance < 0.5f) {
			nextPoint++;
			agent.destination = waypoints[nextPoint % waypoints.Length].position;
		}
	}
}
