using UnityEngine;
using System.Collections;
using System;
using TreeSharpPlus;

public class ApplePicking : MonoBehaviour {

    public GameObject ApplePicker;
    private BehaviorAgent behaviorAgent;
    public Transform[] path;

    protected Node PickApples() {
        return new DecoratorLoop(new Sequence(this.ST_ApproachAndWait(this.path[0]), this.Pick(),
            this.ST_ApproachAndWait(this.path[1]), this.Pick())
        );
    }

    protected Node ST_ApproachAndWait(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(ApplePicker.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position, 1.0f), new LeafWait(1000));
    }

    protected Node Pick()
    {
        return new Sequence(ApplePicker.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("APPLEPICK", 10000));
    }

    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.PickApples());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }
}
