using UnityEngine;
using System.Collections;

public class Director : MonoBehaviour {
	void Start() {
	}
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
				if (hit.transform.tag.Equals("Player")) {
					hit.transform.gameObject.GetComponent<AgentScript>().setSelected();
				} else if (hit.transform.tag.Equals("Obstacle")) {
					hit.transform.gameObject.GetComponent<MovableObstacle>().setSelected();
				} else {
					GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
					foreach (GameObject p in players) {
						p.GetComponent<AgentScript>().moveTo(hit);
					}
				}
			}
		}
	}
}
