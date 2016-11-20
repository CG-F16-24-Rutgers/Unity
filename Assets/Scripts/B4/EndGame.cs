using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGame : MonoBehaviour {

    public Text win_text;
    public Rigidbody rb;

	// Use this for initialization
	void Start () {
        win_text.enabled = false;
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            win_text.enabled = true;
            Application.Quit();
        
        }
    }
}
