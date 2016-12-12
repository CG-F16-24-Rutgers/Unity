using UnityEngine;
using System.Collections;

public class IKSnap : MonoBehaviour {

    public bool use_IK = true;

    public bool left_hand_IK = false;
    public bool right_hand_IK = false;

    public bool left_foot_IK = false;
    public bool right_foot_IK = false;

    private Vector3 left_hand_pos;
    private Vector3 right_hand_pos;

    private Vector3 left_foot_pos;
    private Vector3 right_foot_pos;

    private Vector3 left_hand_opos;
    private Vector3 right_hand_opos;

    public Vector3 left_hand_offset;
    public Vector3 right_hand_offset;

    public Vector3 left_foot_offset;
    public Vector3 right_foot_offset;

    private Quaternion left_hand_rot;
    private Quaternion right_hand_rot;

    private Quaternion left_foot_rot;
    private Quaternion right_foot_rot;

    public Quaternion left_foot_rot_offset;
    public Quaternion right_foot_rot_offset;

    public bool useFootIK = false;

    private Animator anim;
    private float normalized_time;

    public float lh_weight = 1f;
    public float rh_weight = 1f;

    // Use this for initialization
    void Start()
    {

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (use_IK)
        {
            anim.SetBool("useIK", true);
        }
        else {
            anim.SetBool("useIK", false);
        }

        RaycastHit lh_hit;
        RaycastHit rh_hit;

        RaycastHit lf_hit;
        RaycastHit rf_hit;

        //Left Hand IK Check
        if (Physics.Raycast(transform.position + transform.TransformDirection(new Vector3(0.0f, 2.0f, 1.0f)), transform.TransformDirection(new Vector3(-0.5f, -1.0f, 0.0f)), out lh_hit, 1f))
        {

            Vector3 lookAt = Vector3.Cross(-lh_hit.normal, transform.right);
            lookAt = lookAt.y < 0 ? -lookAt : lookAt;

            //Setting true if raycast hits something
            left_hand_IK = true;

            //Setting left_hand_pos to raycast hit points and subtracting the offsets
            left_hand_pos = lh_hit.point - transform.TransformDirection(left_hand_offset);
            //left_hand_rot = Quaternion.FromToRotation(Vector3.forward, lh_hit.normal);
            left_hand_rot = Quaternion.LookRotation(lh_hit.point + lookAt, lh_hit.normal);
        }
        else
            left_hand_IK = false;

        //Right Hand IK Check
        if (Physics.Raycast(transform.position + transform.TransformDirection(new Vector3(0.0f, 2.0f, 1.0f)), transform.TransformDirection(new Vector3(0.5f, -1.0f, 0.0f)), out rh_hit, 1f))
        {

            Vector3 lookAt = Vector3.Cross(-rh_hit.normal, transform.right);
            lookAt = lookAt.y < 0 ? -lookAt : lookAt;

            //Setting true if raycast hits something
            right_hand_IK = true;

            //Setting right_hand_pos to raycast hit points and subtracting the offsets
            right_hand_pos = rh_hit.point - transform.TransformDirection(right_hand_offset);
            //right_hand_rot = Quaternion.FromToRotation(Vector3.forward, rh_hit.normal);
            right_hand_rot = Quaternion.LookRotation(rh_hit.point + lookAt, rh_hit.normal);
        }
        else
            right_hand_IK = false;

        if (useFootIK)
        {
            //Left Foot IK Check
            if (Physics.Raycast(transform.position + transform.TransformDirection(new Vector3(-0.35f, 0.5f, 0.0f)), transform.forward, out lf_hit, 1f))
            {

                left_foot_IK = true;
                left_foot_pos = lf_hit.point - left_foot_offset;
                left_foot_rot = (Quaternion.FromToRotation(Vector3.up, lf_hit.normal)) * left_foot_rot_offset;
            }
            else
                left_foot_IK = false;

            //Right Foot IK Check
            if (Physics.Raycast(transform.position + transform.TransformDirection(new Vector3(0.35f, 0.5f, 0.0f)), transform.forward, out rf_hit, 1f))
            {

                right_foot_IK = true;
                right_foot_pos = rf_hit.point - right_foot_offset;
                right_foot_rot = (Quaternion.FromToRotation(Vector3.up, rf_hit.normal)) * right_foot_rot_offset;
            }
            else
                right_foot_IK = false;
        }
        normalized_time = anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;

        if (anim)
        {

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdlePose"))
            { //Change the name to whatever your "ON LEDGE MOVING" animation is named to...

                float vel = 0.0f;

                if (normalized_time < 0.25f)
                {

                    //Smoothly changing IK Weights from current value to 0 or current value to 1 based on current hand's position (find the specific time based on animation)
                    lh_weight = Mathf.SmoothDamp(lh_weight, 0.0f, ref vel, 2 * Time.deltaTime);
                    rh_weight = Mathf.SmoothDamp(rh_weight, 1.0f, ref vel, 8 * Time.deltaTime);
                }

                else if (normalized_time > 0.25f && normalized_time < 0.5f)
                {

                    //Smoothly changing IK Weights from current value to 0 or current value to 1 based on current hand's position (find the specific time based on animation)
                    lh_weight = Mathf.SmoothDamp(lh_weight, 1.0f, ref vel, 8 * Time.deltaTime);
                    rh_weight = Mathf.SmoothDamp(rh_weight, 0.0f, ref vel, 2 * Time.deltaTime);
                }
            }

            else
            { //Resets the hand weights back to 1.0f, add further animation info here to make sure that weights are 0.0f when in normal player movement

                lh_weight = 1.0f;
                rh_weight = 1.0f;
            }
        }
    }

    void OnDrawGizmos()
    {

        //Left Hand IK Visual Ray
        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(0.0f, 2.0f, 1.0f)), transform.TransformDirection(new Vector3(-0.5f, -1.0f, 0.0f)), Color.green);

        //Right Hand IK Visual Ray
        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(0.0f, 2.0f, 1.0f)), transform.TransformDirection(new Vector3(0.5f, -1.0f, 0.0f)), Color.green);

        //Left Foot IK Visual Ray
        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(-0.5f, 0.5f, 0.0f)), transform.forward, Color.red);

        //Right Foot IK Visual Ray
        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(0.5f, 0.5f, 0.0f)), transform.forward, Color.red);
    }

    void OnAnimatorIK()
    {

        //Setting up IK Weights and positions
        if (use_IK)
        {

            left_hand_opos = anim.GetIKPosition(AvatarIKGoal.LeftHand);
            right_hand_opos = anim.GetIKPosition(AvatarIKGoal.RightHand);

            if (left_hand_IK)
            {

                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, lh_weight);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, left_hand_pos);

                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, left_hand_rot);
            }

            if (right_hand_IK)
            {

                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rh_weight);
                anim.SetIKPosition(AvatarIKGoal.RightHand, right_hand_pos);

                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
                anim.SetIKRotation(AvatarIKGoal.RightHand, right_hand_rot);
            }

            if (left_foot_IK)
            {

                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
                anim.SetIKPosition(AvatarIKGoal.LeftFoot, left_foot_pos);

                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
                anim.SetIKRotation(AvatarIKGoal.LeftFoot, left_foot_rot);
            }

            if (right_foot_IK)
            {

                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
                anim.SetIKPosition(AvatarIKGoal.RightFoot, right_foot_pos);

                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
                anim.SetIKRotation(AvatarIKGoal.RightFoot, right_foot_rot);
            }
        }
    }
}
