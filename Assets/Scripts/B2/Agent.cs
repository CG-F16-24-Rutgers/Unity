using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {
	private bool selected;
	NavMeshAgent agent;
	private Animator animator;
	Vector2 smoothPos, velocity;
	
	public float m_Damping = 0.15f;
	private readonly int m_HashHorizontalPara = Animator.StringToHash("Horizontal");
	private readonly int m_HashVerticalPara = Animator.StringToHash("Vertical");
	
	void Start() {
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		selected = false;
		agent.updatePosition = false;
		smoothPos = Vector2.zero;
		velocity = Vector2.zero;
	}
	void Update() {
		if (selected) {
			float rb = 1.0f - Mathf.Abs(Mathf.Sin(Time.fixedTime * 3.0f));
			transform.Find("Plane").gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Vector4(rb, 1.0f, rb, rb));
		}
		
		Vector3 worldPos = agent.nextPosition - transform.position;
		Vector2 deltaPos = new Vector2(Vector3.Dot(transform.right, worldPos), Vector3.Dot(transform.forward, worldPos));
		float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
		smoothPos = Vector2.Lerp(smoothPos, deltaPos, smooth);
		velocity = smoothPos / Time.deltaTime;
		bool moving = velocity.magnitude > 0.5f && agent.remainingDistance > 0.2f;
		animator.SetBool ("Move", moving);
		animator.SetBool("Run", moving);
		
		Vector2 input = new Vector2(agent.steeringTarget.x, agent.steeringTarget.y).normalized;
		animator.SetFloat(m_HashHorizontalPara, input.x, m_Damping, Time.deltaTime * 10.0f);
		animator.SetFloat(m_HashVerticalPara, input.y, m_Damping, Time.deltaTime * 10.0f);
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
			transform.position = agent.nextPosition;
		}
	}
	void OnAnimatorMove() {
		transform.position = agent.nextPosition;
	}
}