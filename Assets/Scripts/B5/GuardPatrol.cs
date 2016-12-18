using UnityEngine;
using System.Collections;
using System;
using TreeSharpPlus;

public class GuardPatrol : MonoBehaviour {

    public GameObject Guard;
    private BehaviorAgent behaviorAgent;
    public Transform[] path;

    protected Node Patroling()
    {
        return new DecoratorLoop(new Sequence(this.ST_ApproachAndWait(this.path[0]), this.ST_ApproachAndWait(this.path[1]))
        );
    }

    protected Node ST_ApproachAndWait(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(Guard.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position, 1.0f), new LeafWait(1000));
    }

    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.Patroling());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }
}
