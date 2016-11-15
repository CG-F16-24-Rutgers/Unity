using UnityEngine;
using System.Collections;
using System;
using TreeSharpPlus;
using RootMotion;
using RootMotion.FinalIK;

public class ReachObjectInvoker : MonoBehaviour {

    public GameObject Wanderer;

    public FullBodyBipedEffector effector;
    public InteractionObject reach_object;

    public BallScript b;
    public ReachBall rb;

    private BehaviorAgent behaviorAgent;

    protected Node EyeContact(Val<Vector3> ObjectPos)
    {
        

        return new Sequence(Wanderer.GetComponent<BehaviorMecanim>().Node_HeadLook(ObjectPos));
    }

    protected Node ReachObject()
    {
        return new Sequence(new LeafInvoke(() => rb.PickUpTheObject()));
    }

    protected Node DropObject()
    {
        return new LeafInvoke(() => b.pickup = 0);
    }

    protected Node EyeContactAndReach(Val<Vector3> ObjectPos)
    {
        return new Sequence(this.EyeContact(ObjectPos),
            this.ReachObject());
    }

    protected Node ApproachAndOrient(Val<Vector3> ObjectPos)
    {
        return new Sequence(Wanderer.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(ObjectPos, 1.0f),
                            Wanderer.GetComponent<BehaviorMecanim>().Node_OrientTowards(ObjectPos));
    }

    public Node ReachObjectTree() //root node
    {
        Val<Vector3> ObjectPos = Val.V(() => reach_object.transform.position);

        return new Sequence(this.ApproachAndOrient(ObjectPos),
                            this.EyeContactAndReach(ObjectPos), new LeafWait(3000),
                            this.DropObject());
    }

    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.ReachObjectTree());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }
}
