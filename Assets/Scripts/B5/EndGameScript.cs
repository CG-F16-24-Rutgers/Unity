using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameScript : MonoBehaviour {

    public Text win_text;
    public Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        win_text.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Marker"))
        {
            win_text.text = "You Win";
            win_text.enabled = true;
            wait(5);
            Application.Quit();

        }
    }

    public IEnumerator wait(float delayInSecs)
    {
        yield return new WaitForSecondsRealtime(delayInSecs);
    }
}
