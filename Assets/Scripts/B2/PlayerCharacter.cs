using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {
	public Animator animator;
	public Rigidbody r_body;
	public NavMeshAgent agent;

	void Start() {
		animator.GetComponent<Animator> ();
		r_body.freezeRotation = true;
		agent.GetComponent<NavMeshAgent> ();
	}

	void Update() {
		if (Input.GetKey ("space")) {
			animator.SetBool ("Jump", true);
			agent.enabled = true;
		} else {
			animator.SetBool ("Jump", false);
			agent.enabled = false;
		}

		if (Input.GetKey ("w") || Input.GetKey ("a") || Input.GetKey ("s") || Input.GetKey ("d")) {
			animator.SetBool ("Move", true);
			if (Input.GetKey (KeyCode.LeftControl)) {
				animator.SetBool ("Run", true);
			} else {
				animator.SetBool ("Run", false);
			}
		} else {
			animator.SetBool ("Move", false);
			animator.SetBool ("Run", false);
		}

		/*if (Input.GetKey (KeyCode.LeftControl)) {
			animator.SetBool ("Run", true);
		} else {
			animator.SetBool ("Run", false);
		}*/
	}
}
