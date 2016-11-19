using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;

public class BallScript : MonoBehaviour {

    public InteractionObject ball;
    public int pickup;  

    private float hold_weight;

    IEnumerator Pickup() {
        while (hold_weight < 1.0f) {
            hold_weight += Time.deltaTime;
            yield return null;
        }
    }

    void Update()
    {
        //if (pickup == 1) StartCoroutine(Pickup());
        if (pickup == 0) StartCoroutine(Drop());
    }

    IEnumerator Drop() {
        GetComponent<Rigidbody>().isKinematic = false;
        transform.parent = null;

        while (hold_weight > 0f) {
            hold_weight -= Time.deltaTime * 3f;
            yield return null;
        }
    }
}
