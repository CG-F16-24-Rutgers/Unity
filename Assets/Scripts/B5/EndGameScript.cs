using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameScript : MonoBehaviour {

    public Text win_text;
    public Text story_text;
    public Image dialogue_box;
    public Rigidbody rb;
    public bool in_trigger;
    public bool ending;
    public Collider col;

    public int story;
    public int determine;

    // Use this for initialization
    void Start()
    {
        win_text.enabled = false;
        story_text.enabled = false;
        dialogue_box.enabled = false;
        in_trigger = false;
        ending = false;

        story = 0;
        determine = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        in_trigger = true;
        col = other;

        if (other.gameObject.CompareTag("Finish"))
        {
            win_text.text = "You Win";
            win_text.enabled = true;
            wait();
            Application.Quit();

        }

        if (other.gameObject.CompareTag("Marker"))
        {
            story_text.text = "Hello Traveler, welcome to the inn.\nPlease enjoy your stay.";
            dialogue_box.enabled = true;
            story_text.enabled = true;
        }

        if (other.gameObject.CompareTag("Part3"))
        {
            ending = true;
        }

        if (story == 1)
        {
            if (other.gameObject.CompareTag("Part1"))
            {
                determine = story;
                story_text.text = "Hello Traveler, welcome.\nPlease meet me by the wall.";
                dialogue_box.enabled = true;
                story_text.enabled = true;
            }
        }
        if (story == 2)
        {
            if (other.gameObject.CompareTag("Part2"))
            {
                determine = story;
                story_text.text = "Try to climb this wall.\nIf you get stuck, try to backtrack\nyour position then go forward again.\nWhen you are done, meet me\n by the exit of the village.";
                dialogue_box.enabled = true;
                story_text.enabled = true;
            }
        }
        if (story == 3 && ending)
        {
            if (other.gameObject.CompareTag("Part4"))
            {
                determine = story;
                story_text.text = "You have made it to the top, and\nyou are now ready to\nface your next challenge.\nHead to the top of that tower.\nYou may find the answers you seek.";
                dialogue_box.enabled = true;
                story_text.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        story_text.enabled = false;
        dialogue_box.enabled = false;
        in_trigger = false;
        determine = 0;
    }

    public IEnumerator wait()
    {
        float delayInSecs = 5;
        float increment_to_remove = 0.5f;
        while (delayInSecs > 0)
        {
            yield return new WaitForSeconds(increment_to_remove);
            delayInSecs -= increment_to_remove;
        }
    }
}
