using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;

public class ReachBall : MonoBehaviour {

    public FullBodyBipedEffector effector;
    public InteractionObject interaction_object;

    private InteractionSystem interaction_system;

    void Awake() {
        interaction_system = GetComponent<InteractionSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void PickUpTheObject() {
        interaction_system.StartInteraction(effector, interaction_object, true);
    }
}
