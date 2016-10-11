using UnityEngine;
using System.Collections;

public class ThirdPersonCam : MonoBehaviour {
	public GameObject player;
	void Start() {
		transform.SetParent(player.transform);
	}
}
