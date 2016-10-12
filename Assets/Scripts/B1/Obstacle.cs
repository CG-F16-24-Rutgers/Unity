using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
	bool movingRight;
	float moveTimer;
	float speed = 5.0f;
	void Start() {
		moveTimer = 0.0f;
		movingRight = false;
	}
	void Update() {
		if (movingRight) {
			transform.Translate(Vector3.right * speed * Time.deltaTime);
		} else {
			transform.Translate(Vector3.left * speed * Time.deltaTime);
		}
		moveTimer += speed * Time.deltaTime;
		if (moveTimer >= 20.0f) {
			movingRight = !movingRight;
			moveTimer = 0.0f;
		}
	}
}
