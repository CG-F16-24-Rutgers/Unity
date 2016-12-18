using UnityEngine;
using System.Collections;
using System;
using TreeSharpPlus;

public class ConversationLoop : MonoBehaviour {

    public GameObject Wanderer;
    public GameObject Friend;
    private BehaviorAgent behaviorAgent;

    protected Node EyeContact(Val<Vector3> WanderPos, Val<Vector3> FriendPos)
    {
        Vector3 height = new Vector3(0.0f, 1.85f, 0.0f);

        Val<Vector3> WandererHead = Val.V(() => WanderPos.Value + height);
        Val<Vector3> FriendHead = Val.V(() => FriendPos.Value + height);

        return new SequenceParallel(Friend.GetComponent<BehaviorMecanim>().Node_HeadLook(WandererHead),
                                    Wanderer.GetComponent<BehaviorMecanim>().Node_HeadLook(FriendHead));
    }

    protected Node Converse()
    {
        return new Sequence(Wanderer.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("ACKNOWLEDGE", 2000),
                            Friend.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADSHAKE", 2000),
                            Wanderer.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("BEINGCOCKY", 2000),
                            Friend.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADNOD", 2000));
    }

    protected Node EyeContactAndConverse(Val<Vector3> WanderPos, Val<Vector3> FriendPos)
    {
        return new Sequence(this.EyeContact(WanderPos, FriendPos),
            this.Converse());
    }

    protected Node ApproachAndOrient(Val<Vector3> WanderPos, Val<Vector3> FriendPos)
    {
        return new Sequence(Friend.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(WanderPos, 2.0f),
            new SequenceParallel(Friend.GetComponent<BehaviorMecanim>().Node_OrientTowards(WanderPos),
                                 Wanderer.GetComponent<BehaviorMecanim>().Node_OrientTowards(FriendPos)));
    }

    public Node ConversationTree() //root node
    {
        Val<Vector3> WandererPos = Val.V(() => Wanderer.transform.position);
        Val<Vector3> FriendPos = Val.V(() => Friend.transform.position);

        return new Sequence(this.ApproachAndOrient(WandererPos, FriendPos),
                            this.EyeContactAndConverse(WandererPos, FriendPos), new DecoratorLoop(this.EyeContactAndConverse(WandererPos, FriendPos)));
    }

    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.ConversationTree());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }
}
