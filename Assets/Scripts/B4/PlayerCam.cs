using UnityEngine;
using System.Collections;

public class PlayerCam : MonoBehaviour {
	public GameObject target;
	public float damping = 1;
	Vector3 offset;

	void Start() {
		offset = target.transform.position - transform.position;
	}

	void LateUpdate() {
		float currentAngle = transform.eulerAngles.y;
		float desiredAngle = target.transform.eulerAngles.y;
		float angle = Mathf.LerpAngle(currentAngle, desiredAngle, damping);

		Quaternion rotation = Quaternion.Euler(0, angle, 0);
		transform.position = target.transform.position - (rotation * offset);

		transform.LookAt(target.transform);
	}

	/*public Transform target;
	public float look_smooth = 0.09f;
	public Vector3 offset = new Vector3 (0, 6, -8);
	public float x_tilt = 10;

	Vector3 destination = Vector3.zero;
	Locomotion_SMB locomotion;
	float rotate_vel = 0;

	void Start() {
		SetCameraTarget (target);
	}

	void SetCameraTarget(Transform t) {
		target = t;

		if(target != null) {
			if (target.GetComponent<Locomotion_SMB> ()) {
				locomotion = target.GetComponent<Locomotion_SMB> ();
			} else {
				Debug.LogError ("Your camera's target needs a SMB.");
			}
		} else {
			Debug.LogError ("Your camera needs a target.");
		}
	}

	void LateUpdate() {
		MoveToTarget ();
		LookAtTarget ();
	}

	void MoveToTarget() {
		destination = offset;
		destination += target.position;
		transform.position = destination;
	}

	void LookAtTarget() {
		float euler_y_angle = Mathf.SmoothDampAngle (transform.eulerAngles.y, target.eulerAngles.y, ref rotate_vel, look_smooth);
	}*/
}
