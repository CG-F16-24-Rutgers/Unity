using UnityEngine;
using System.Collections;

public class B4Camera : MonoBehaviour {
	public GameObject player;
	private Vector3 offset;
	void Start () {
		offset = player.transform.position - transform.position;
	}
	void Update () {
	}
	void LateUpdate() {
		transform.position = player.transform.position - offset;
	}
}
