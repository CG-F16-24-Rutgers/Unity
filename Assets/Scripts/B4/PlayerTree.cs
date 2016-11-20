using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion;
using RootMotion.FinalIK;

public class PlayerTree : MonoBehaviour {

    public Transform wander1;
    public Transform wander2;
    public GameObject player;
    public Transform[] path1;
    public Transform[] path2;
    public Transform path3;

    public OpenCloseDoor d;

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
        /*Node roaming = new Sequence(this.DoorOpen_ApproachAndWait(this.wander1), this.DoorClose_ApproachAndWait(this.wander2),
                            new DecoratorLoop(
                                new Sequence(this.AssertUserClicked())));*/
        Node roaming = new Sequence(this.DoorOpen_ApproachAndWait(this.wander1), this.DoorClose_ApproachAndWait(this.wander2),
                            new SequenceShuffle(
                                new Sequence(this.ST_ApproachAndWait(this.path1[0]), this.ST_ApproachAndWait(this.path1[1]), this.ST_ApproachAndWait(this.path1[2]), this.ST_ApproachAndWait(this.path1[3]), this.ST_ApproachAndWait(this.path1[4])),
                                new Sequence(this.ST_ApproachAndWait(this.path2[0]), this.ST_ApproachAndWait(this.path2[1]), this.ST_ApproachAndWait(this.path2[2])),
                                new Sequence(this.ST_ApproachAndWait(this.path3))));
        return roaming;
    }

    protected Node DoorOpen_ApproachAndWait(Transform target)
    {
        return new Sequence(ST_ApproachAndWait(target), new LeafInvoke(() => d.open = true));
    }

    protected Node DoorClose_ApproachAndWait(Transform target)
    {
        return new Sequence(ST_ApproachAndWait(target), new LeafInvoke(() => d.open = false));
    }

    //ApproachAndWait SubTree
    protected Node ST_ApproachAndWait(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(player.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    //Click to Move
    void MoveToPointOnMap()
    {
        Val<Ray> ray = Val.V(() => Camera.main.ScreenPointToRay(Input.mousePosition));
        RaycastHit hit;
        Physics.Raycast(ray.Value, out hit, 100);
        Val.V(() => Physics.Raycast(ray.Value, out hit, 100));
        Val<GameObject> player_val = Val.V(() => player);

        if (player_val.Value.GetComponent<CharacterMecanim>().Body.NavCanReach((Val.V(() => hit.point)).Value) == false)
            {
                return;

            }
            player_val.Value.GetComponent<SteeringController>().Target = (Val.V(() => hit.point)).Value;
    }

    protected Node CheckClickInMap()
    {
        Val<Ray> ray = Val.V(() => Camera.main.ScreenPointToRay(Input.mousePosition));
        RaycastHit hit;
        return new LeafAssert(() => Physics.Raycast(ray.Value, out hit, 100));
    }

    protected Node AssertClickInMap()
    {
        return
            new Sequence(new DecoratorInvert(new DecoratorLoop(new DecoratorInvert(new Sequence(this.CheckClickInMap())))),
                         new LeafInvoke(() => MoveToPointOnMap())
                         );
    }

    protected Node AssertUserClicked()
    {
        return new DecoratorLoop(new Sequence(new DecoratorInvert(new DecoratorLoop(new DecoratorInvert(new Sequence(this.CheckClicked())))),
                                              AssertClickInMap()));
    }

    protected Node CheckClicked()
    {
        return new LeafAssert(() => Input.GetButton("Fire1"));
    }

}
