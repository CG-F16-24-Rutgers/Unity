using UnityEngine;
using System.Collections;

public class AgentScript : MonoBehaviour {
	private bool selected;
	private float greenTime;
	NavMeshAgent agent;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
		selected = false;
		greenTime = 0.0f;
	}
	void Update() {
		if (selected) {
			greenTime += Time.deltaTime;
			float rb = 1.0f - Mathf.Abs(Mathf.Sin(greenTime * 3.0f));
			GetComponent<Renderer>().material.SetColor("_Color", new Vector4(rb, 1.0f, rb, 1.0f));
		}
	}
	public void setSelected() {
		selected = !selected;
		GetComponent<Renderer>().material.SetColor("_Color", Color.white);
	}
	public void moveTo(RaycastHit hit) {
		if (selected) {
			GetComponent<Renderer>().material.SetColor("_Color", Color.white);
			selected = false;
			agent.destination = hit.point;
		}
	}
}