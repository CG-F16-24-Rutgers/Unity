using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour {

    public GameObject door;
    private bool opened;
    Animation anim;

    // Use this for initialization
    void Start () {
        opened = true;
        anim = door.GetComponent<Animation>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (opened)
        {
            anim["Open_Anim"].speed = 1;
            anim["Open_Anim"].time -= anim["Open_Anim"].length;
            anim.Play();
            opened = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!opened)
        {
            anim["Open_Anim"].speed = -1;
            anim["Open_Anim"].time = anim["Open_Anim"].length;
            anim.Play();
            opened = true;
        }
    }
}
