using UnityEngine;
using System.Collections;

public class SphereMover : MonoBehaviour
{
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        float r = Mathf.Abs(Mathf.Sin(Time.fixedTime * 3.0f));
        if (Input.GetKey("up"))
        {
            transform.position -= new Vector3(0.0f, 0.0f, Time.deltaTime * 8.0f);
        }
        if (Input.GetKey("down"))
        {
            transform.position += new Vector3(0.0f, 0.0f, Time.deltaTime * 8.0f);
        }
        if (Input.GetKey("left"))
        {
            transform.position += new Vector3(Time.deltaTime * 8.0f, 0.0f, 0.0f);
        }
        if (Input.GetKey("right"))
        {
            transform.position -= new Vector3(Time.deltaTime * 8.0f, 0.0f, 0.0f);
        }
    }
}