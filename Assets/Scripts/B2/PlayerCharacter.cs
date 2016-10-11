using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {
	void Start() {
	}
	void Update() {
		if (Input.GetKey("up")) {
			transform.position += transform.forward * Time.deltaTime * 4.0f;
		}
		if (Input.GetKey("down")) {
			transform.position -= transform.forward * Time.deltaTime * 4.0f;
		}
		if (Input.GetKey("left")) {
			transform.Rotate(Vector3.up * Time.deltaTime * -64.0f);
		}
		if (Input.GetKey("right")) {
			transform.Rotate(Vector3.up * Time.deltaTime * 64.0f);
		}
	}
}
