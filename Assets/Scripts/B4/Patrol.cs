using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour {
	public Transform[] waypoints;
	private NavMeshAgent agent;
	private int nextPoint;
	private GameObject player;
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		nextPoint = 0;
		agent.destination = waypoints[0].position;
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject p in players) {
			player = p;
		}
	}
	void Update () {
		bool foundPlayer = false;
		Vector3 dir = transform.position - player.transform.position;
		dir = transform.InverseTransformDirection(dir);
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		Debug.Log(angle);
		if (Mathf.Abs(angle) > 135.0f && Mathf.Abs(angle) < 225.0f) {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 100)) {
				if (hit.transform == player.transform) {
					agent.destination = player.transform.position;
					foundPlayer = true;
				}
			}
		}
		if (!foundPlayer) {
			if (agent.remainingDistance < 0.5f) {
				nextPoint++;
				agent.destination = waypoints[nextPoint % waypoints.Length].position;
			}
		}
	}
}
