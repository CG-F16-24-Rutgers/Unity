using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class _MegaScript : MonoBehaviour {

    public Transform wander1;
    public Transform wander2;
    public Transform wander3;

    public GameObject main_char;
    public GameObject friend1;

    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    //Out Most SubTree
    protected Node ST_ApproachAndWait(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(main_char.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    protected Node BuildTreeRoot()
    {
        Node roaming = new DecoratorLoop(
                        new SequenceShuffle(
                            new Sequence(
                                this.ST_ApproachAndWait(this.wander1), this.ConversationTree()),
                            new Sequence(this.ST_ApproachAndWait(this.wander2))
                            ));
        return roaming;
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

    public Node ConversationTree() //root node of subtree
    {
        Val<Vector3> WandererPos = Val.V(() => main_char.transform.position);
        Val<Vector3> FriendPos = Val.V(() => friend1.transform.position);

        return new Sequence(this.ApproachAndOrient(WandererPos, FriendPos),
                            this.EyeContactAndConverse(WandererPos, FriendPos));
    }
}
