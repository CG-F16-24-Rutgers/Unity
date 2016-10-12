using UnityEngine;
using System.Collections;

public class MovableObstacle : MonoBehaviour {
	private bool selected;
	NavMeshAgent agent;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
		selected = false;
	}
	void Update() {
		if (selected) {
			float r = Mathf.Abs(Mathf.Sin(Time.fixedTime * 3.0f));
			GetComponent<Renderer>().material.SetColor("_Color", new Vector4(r, 0.0f, 0.0f, 1.0f));
			if (Input.GetKey("up")) {
				transform.position -= new Vector3(0.0f, 0.0f, Time.deltaTime * 8.0f);
			}
			if (Input.GetKey("down")) {
				transform.position += new Vector3(0.0f, 0.0f, Time.deltaTime * 8.0f);
			}
			if (Input.GetKey("left")) {
				transform.position += new Vector3(Time.deltaTime * 8.0f, 0.0f, 0.0f);
			}
			if (Input.GetKey("right")) {
				transform.position -= new Vector3(Time.deltaTime * 8.0f, 0.0f, 0.0f);
			}
		}
	}
	public void setSelected() {
		selected = !selected;
		GetComponent<Renderer>().material.SetColor("_Color", Color.red);
	}
}
