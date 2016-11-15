using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion;
using RootMotion.FinalIK;

public class _MegaScript : MonoBehaviour {

    public Transform wander1;
    public Transform wander2;
    public Transform wander3;
    public Transform wander4;
    public Transform wander5;

    public GameObject main_char;
    public GameObject friend1;

    public FullBodyBipedEffector effector;
    public InteractionObject reach_object;

    public BallScript b;
    public ReachBall rb;
    public DoorScript d;

    private BehaviorAgent behaviorAgent;

    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    //Outter Most SubTree
    protected Node BuildTreeRoot()
    {
        Node roaming = new Sequence(this.DoorOpen_ApproachAndWait(this.wander4), this.DoorClose_ApproachAndWait(this.wander5),
                            new DecoratorLoop(
                                new SequenceShuffle(
                                    new Sequence(
                                        this.ST_ApproachAndWait(this.wander1), this.ConversationTree()),
                                    new Sequence(
                                        this.ST_ApproachAndWait(this.wander2)),
                                    new Sequence(ST_ApproachAndWait(this.wander3), this.ReachObjectTree())
                                    )));
        return roaming;
    }

    protected Node DoorOpen_ApproachAndWait(Transform target)
    {
        return new Sequence(ST_ApproachAndWait(target), new LeafInvoke (() => d.open = true));
    }

    protected Node DoorClose_ApproachAndWait(Transform target)
    {
        return new Sequence(ST_ApproachAndWait(target), new LeafInvoke(() => d.open = false));
    }

    //ApproachAndWait SubTree
    protected Node ST_ApproachAndWait(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(main_char.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    //ConversationInvoker SubTree

    protected Node EyeContact(Val<Vector3> WanderPos, Val<Vector3> FriendPos)
    {
        Vector3 height = new Vector3(0.0f, 1.85f, 0.0f);

        Val<Vector3> WandererHead = Val.V(() => WanderPos.Value + height);
        Val<Vector3> FriendHead = Val.V(() => FriendPos.Value + height);

        return new SequenceParallel(friend1.GetComponent<BehaviorMecanim>().Node_HeadLook(WandererHead),
                                    main_char.GetComponent<BehaviorMecanim>().Node_HeadLook(FriendHead));
    }

    protected Node Converse()
    {
        return new Sequence(main_char.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("ACKNOWLEDGE", 2000),
                            friend1.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADSHAKE", 2000),
                            main_char.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("BEINGCOCKY", 2000),
                            friend1.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADNOD", 2000));
    }

    protected Node EyeContactAndConverse(Val<Vector3> WanderPos, Val<Vector3> FriendPos)
    {
        return new Sequence(this.EyeContact(WanderPos, FriendPos),
            this.Converse());
    }

    protected Node ApproachAndOrient(Val<Vector3> WanderPos, Val<Vector3> FriendPos)
    {
        return new Sequence(main_char.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(FriendPos, 1.0f),
            new SequenceParallel(friend1.GetComponent<BehaviorMecanim>().Node_OrientTowards(WanderPos),
                                 main_char.GetComponent<BehaviorMecanim>().Node_OrientTowards(FriendPos)));
    }

    protected Node ConversationTree() //root node of subtree
    {
        Val<Vector3> WandererPos = Val.V(() => main_char.transform.position);
        Val<Vector3> FriendPos = Val.V(() => friend1.transform.position);

        return new Sequence(this.ApproachAndOrient(WandererPos, FriendPos),
                            this.EyeContactAndConverse(WandererPos, FriendPos));
    }

    //ReachObjectInvoker SubTree
    protected Node EyeContact(Val<Vector3> ObjectPos)
    {
        return new Sequence(main_char.GetComponent<BehaviorMecanim>().Node_HeadLook(ObjectPos));
    }

    protected Node ReachObject()
    {
        return new LeafInvoke(() => rb.PickUpTheObject());
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
        return new Sequence(main_char.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(ObjectPos, 1.0f),
                            main_char.GetComponent<BehaviorMecanim>().Node_OrientTowards(ObjectPos));
    }

    protected Node ReachObjectTree() //root node
    {
        Val<Vector3> ObjectPos = Val.V(() => reach_object.transform.position);

        return new Sequence(this.ApproachAndOrient(ObjectPos),
                            this.EyeContactAndReach(ObjectPos), new LeafWait(3000),
                            this.DropObject());
    }
}
