using UnityEngine;
using System.Collections;

public class B2Director : MonoBehaviour {
	void Start() {
	}
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
				GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
				foreach (GameObject p in players) {
					p.GetComponent<Agent>().moveTo(hit);
				}
			}
		} else if (Input.GetMouseButtonDown(1)) {
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
				if (hit.transform.tag.Equals("Player")) {
					hit.transform.gameObject.GetComponent<Agent>().setSelected();
				}
			}
		}
	}
}
