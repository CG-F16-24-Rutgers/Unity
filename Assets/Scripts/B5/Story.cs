using UnityEngine;
using System.Collections;
using System;
using TreeSharpPlus;

public class Story : MonoBehaviour {
    public GameObject Story_NPC;
    public GameObject Player;
    private BehaviorAgent behaviorAgent;
    public EndGameScript egs;
    public Transform[] path;

    public Node OurStory() //root node
    {
        return new Sequence(new LeafInvoke(() => egs.story = 1), this.AssertAndHaveConvo(),
            new LeafInvoke(() => egs.story = 2), this.ST_Approach(path[0]), this.AssertAndHaveConvo(),
            new LeafInvoke(() => egs.story = 3), this.ST_Approach(path[1]), this.AssertAndHaveConvo(),
            new LeafInvoke(() => egs.story = 4)
        );
    }

    protected Node AssertAndHaveConvo()
    {
        return new Sequence(
            new DecoratorInvert(new DecoratorLoop(new DecoratorInvert(new Sequence(new LeafAssert(() => egs.in_trigger && egs.story == egs.determine))))),
            this.ConversationTree()
        );
    }

    protected Node EyeContact(Val<Vector3> PlayerPos)
    {
        Vector3 height = new Vector3(0.0f, 1.85f, 0.0f);

        Val<Vector3> PlayerHead = Val.V(() => PlayerPos.Value + height);

        return new Sequence(Story_NPC.GetComponent<BehaviorMecanim>().Node_HeadLook(PlayerHead));
    }

    protected Node Converse()
    {
        return new Sequence(Story_NPC.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("ACKNOWLEDGE", 2000),
                            Story_NPC.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADSHAKE", 2000),
                            Story_NPC.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("BEINGCOCKY", 2000),
                            Story_NPC.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADNOD", 2000));
    }

    protected Node EyeContactAndConverse(Val<Vector3> PlayerPos)
    {
        return new Sequence(this.EyeContact(PlayerPos),
            this.Converse());
    }

    protected Node ApproachAndOrient(Val<Vector3> PlayerPos)
    {
        return new Sequence(Story_NPC.GetComponent<BehaviorMecanim>().Node_OrientTowards(PlayerPos));
    }

    public Node ConversationTree()
    {
        Val<Vector3> PlayerPos = Val.V(() => Player.transform.position);

        return new Sequence(this.ApproachAndOrient(PlayerPos),
                            this.EyeContactAndConverse(PlayerPos));
    }

    protected Node ST_Approach(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(Story_NPC.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position, 1.0f));
    }

    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.OurStory());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }
}
