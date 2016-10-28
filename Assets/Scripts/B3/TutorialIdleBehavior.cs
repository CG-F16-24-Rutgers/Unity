using UnityEngine;
using System.Collections;
using System;
using TreeSharpPlus;

public class TutorialIdleBehavior : MonoBehaviour {

    public GameObject participant;
    private BehaviorAgent behaviorAgent;

    protected Node BuildTreeRoot() {
        return
         new DecoratorLoop(
             new Sequence(participant.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("texting", 3000), new LeafWait(10000)));
    }

    // Use this for initialization
    void Start () {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
