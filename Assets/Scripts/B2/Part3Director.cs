using UnityEngine;
using System.Collections;

public class Part3Director : MonoBehaviour {
	private Vector3 goal = new Vector3(20.0f, 2.1f, 0.0f);
	private bool unselected;
	void Start() {
		unselected = true;
	}
	void Update() {
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject p in players) {
			if (unselected) {
				p.GetComponent<Agent>().setSelected();
			}
			p.GetComponent<Agent>().moveTo(goal);
		}
		unselected = false;
	}
}
