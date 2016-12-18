using UnityEngine;
using System.Collections;
using System;
using TreeSharpPlus;

public class WoodCutting : MonoBehaviour {

    public GameObject WoodCutter;
    private BehaviorAgent behaviorAgent;
    public Transform[] path;

    protected Node CutWood()
    {
        return new DecoratorLoop(new Sequence(this.ST_ApproachAndWait(this.path[0]), this.Cut(),
            this.ST_ApproachAndWait(this.path[1]), this.Cut(),
            this.ST_ApproachAndWait(this.path[2]), this.Cut())
        );
    }

    protected Node ST_ApproachAndWait(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(WoodCutter.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position, 1.0f), new LeafWait(1000));
    }

    protected Node Cut()
    {
        return new Sequence(WoodCutter.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("WOODCUT", 10000));
    }

    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.CutWood());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }
}
