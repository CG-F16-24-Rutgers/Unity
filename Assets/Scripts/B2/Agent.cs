using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {
	private bool selected;
	NavMeshAgent agent;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
		selected = false;
	}
	void Update() {
		if (selected) {
			float rb = 1.0f - Mathf.Abs(Mathf.Sin(Time.fixedTime * 3.0f));
			transform.Find("Plane").gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Vector4(rb, 1.0f, rb, 1.0f));
		}
	}
	public void setSelected() {
		selected = !selected;
		transform.Find("Plane").gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
	}
	public void moveTo(RaycastHit hit) {
		if (selected) {
			transform.Find("Plane").gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
			selected = false;
			agent.destination = hit.point;
		}
	}
}