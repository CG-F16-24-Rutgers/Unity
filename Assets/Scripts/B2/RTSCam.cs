using UnityEngine;
using System.Collections;

public class RTSCam : MonoBehaviour {
	void Start () {
	}
	void Update () {
		if (Input.GetKey("w")) {
			transform.Translate(new Vector3(4.0f, 0.0f, 4.0f) * Time.deltaTime, Space.World);
		} else if (Input.GetKey("s")) {
			transform.Translate(new Vector3(-4.0f, 0.0f, -4.0f) * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("d")) {
			transform.Translate(new Vector3(4.0f, 0.0f, -4.0f) * Time.deltaTime, Space.World);
		} else if (Input.GetKey("a")) {
			transform.Translate(new Vector3(-4.0f, 0.0f, 4.0f) * Time.deltaTime, Space.World);
		}
		if (transform.position.x < -50.0f) {
			transform.Translate(new Vector3(-50.0f - transform.position.x, 0.0f, 0.0f) * Time.deltaTime, Space.World);
		} else if (transform.position.x > 40.0f) {
			transform.Translate(new Vector3(40.0f - transform.position.x, 0.0f, 0.0f) * Time.deltaTime, Space.World);
		}
		if (transform.position.z < -50.0f) {
			transform.Translate(new Vector3(0.0f, 0.0f, -50.0f - transform.position.z) * Time.deltaTime, Space.World);
		} else if (transform.position.z > 40.0f) {
			transform.Translate(new Vector3(0.0f, 0.0f, 40.0f - transform.position.z) * Time.deltaTime, Space.World);
		}
		float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
		if (mouseScroll > 0.0f) {
			if (transform.position.y > 2.5f) {
				transform.Translate(new Vector3(0.0f, -1.0f, 0.0f), Space.World);
			}
		} else if (mouseScroll < 0.0f) {
			if (transform.position.y < 10.5f) {
				transform.Translate(new Vector3(0.0f, 1.0f, 0.0f), Space.World);
			}
		}
	}
}
