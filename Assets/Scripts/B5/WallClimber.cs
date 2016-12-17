using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class WallClimber : MonoBehaviour {

    public float ClimbForce;
    public float smallestEdge = 0.25f;
    public float CoolDown = 0.15f;
    public float minDistance;
    public float MaxAngle = 30.0f;
    public float ClimbRange = 2.0f;
    public float JumpForce = 1.0f;
    public Climbingsort currentSort;
    public ThirdPersonUserControl TPUC;
    public ThirdPersonCharacter TPC;

    public Transform HandTransform;
    public Vector3 VerticalHandOffset;
    public Vector3 HorizontalHandOffset;
    public Vector3 FallHandsOffset;
    public Vector3 RaycastPosition;

    public LayerMask SpotLayer;
    public LayerMask CurrentSpotLayer;
    public LayerMask CheckLayersForObstacle;
    public LayerMask CheckLayersReachable;

    private Rigidbody rb;
    private Animator anim;

    private Vector3 TargetPoint;
    private Vector3 TargetNormal;
    private int horizontal;
    private int vertical;


    private float lasttime;
    private float BeginDistance;
    private RaycastHit hit;
    private Quaternion oldRotation;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (currentSort == Climbingsort.Walking && Input.GetAxis("Vertical") > 0)
            StartClimbing();

        if (currentSort == Climbingsort.Climbing)
            Climb();

        UpdateStats();

        if (currentSort == Climbingsort.ClimbingTowardsPoint || currentSort == Climbingsort.ClimbingTowardsPlateau)
            MoveTowardsPoint();

        if (currentSort == Climbingsort.Jumping || currentSort == Climbingsort.Falling)
            Jumping();
	}

    public void UpdateStats() {
        if (currentSort != Climbingsort.Walking  &&  TPC.m_IsGrounded && currentSort != Climbingsort.ClimbingTowardsPoint) {
            currentSort = Climbingsort.Walking;
            TPUC.enabled = true;
            rb.isKinematic = false;
        }

        if (currentSort == Climbingsort.Walking && !TPC.m_IsGrounded)
            currentSort = Climbingsort.Jumping;

        if(currentSort == Climbingsort.Walking && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
            CheckForClimbStart();

    }

    public void StartClimbing() {
        if (Physics.Raycast(transform.position + transform.rotation * RaycastPosition, transform.forward, 0.4f) && Time.time - lasttime > CoolDown && currentSort == Climbingsort.Walking) {
            if (currentSort == Climbingsort.Walking) {
                rb.AddForce(transform.up * JumpForce);
            }

            lasttime = Time.time;
        }
    }

    public void Jumping() {
        if (rb.velocity.y < 0 && currentSort != Climbingsort.Falling) {
            currentSort = Climbingsort.Falling;
            oldRotation = transform.rotation;
        }

        if (rb.velocity.y > 0 && currentSort != Climbingsort.Jumping) {
            currentSort = Climbingsort.Jumping;
        }

        if (currentSort == Climbingsort.Jumping) 
            CheckForSpots(HandTransform.position + FallHandsOffset, -transform.up, 0.1f, Checkingsort.normal);

        if (currentSort == Climbingsort.Falling) {
            CheckForSpots(HandTransform.position + FallHandsOffset + transform.rotation * new Vector3(0.02f, -0.6f, 0.0f), -transform.up, 0.4f, Checkingsort.normal);
            transform.rotation = oldRotation;
        }
    }

    public void Climb() {
        if (Time.time - lasttime > CoolDown && currentSort == Climbingsort.Climbing)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                CheckForSpots(HandTransform.position + transform.rotation * VerticalHandOffset + transform.up * ClimbRange, -transform.up, ClimbRange, Checkingsort.normal);

                if (currentSort != Climbingsort.ClimbingTowardsPoint)
                    CheckForPlateau();
            }

            if (Input.GetAxis("Vertical") < 0)
            {
                CheckForSpots(HandTransform.position - transform.rotation * (VerticalHandOffset + new Vector3(0.0f, 0.3f, 0.0f)), -transform.up, ClimbRange, Checkingsort.normal);

                if (currentSort != Climbingsort.ClimbingTowardsPoint)
                {
                    rb.isKinematic = false;
                    TPUC.enabled = true;
                    currentSort = Climbingsort.Falling;
                    oldRotation = transform.rotation;
                }
            }

            if (Input.GetAxis("Horizontal") != 0)
            {
                CheckForSpots(HandTransform.position + transform.rotation * HorizontalHandOffset, transform.right * Input.GetAxis("Horizontal") - transform.up / 3.5f, ClimbRange / 2.0f, Checkingsort.normal);

                if (currentSort != Climbingsort.ClimbingTowardsPoint)
                {
                    CheckForSpots(HandTransform.position + transform.rotation * HorizontalHandOffset, transform.right * Input.GetAxis("Horizontal") - transform.up / 1.5f, ClimbRange / 3.0f, Checkingsort.normal);
                }

                if (currentSort != Climbingsort.ClimbingTowardsPoint)
                {
                    CheckForSpots(HandTransform.position + transform.rotation * HorizontalHandOffset, transform.right * Input.GetAxis("Horizontal") - transform.up / 6.0f, ClimbRange / 1.5f, Checkingsort.normal);
                }

                if (currentSort != Climbingsort.ClimbingTowardsPoint)
                {
                    int hor = 0;

                    if (Input.GetAxis("Horizontal") < 0)
                        hor = -1;
                    if (Input.GetAxis("Horizontal") > 0)
                        hor = 1;

                    CheckForSpots(HandTransform.position + transform.rotation * HorizontalHandOffset + transform.right * hor * smallestEdge / 4, transform.forward - transform.up * 2, ClimbRange / 3.0f, Checkingsort.turning);

                    if (currentSort != Climbingsort.ClimbingTowardsPoint)
                        CheckForSpots(HandTransform.position + transform.rotation * HorizontalHandOffset + transform.right * 0.2f, transform.forward - transform.up * 2 + transform.right * hor / 1.5f, ClimbRange / 3.0f, Checkingsort.turning);
                }
            }
        }
    }

    public void CheckForSpots(Vector3 Spotlocation, Vector3 dir, float range, Checkingsort sort) {
        bool foundspot = false;

        if (Physics.Raycast(Spotlocation - transform.right * smallestEdge / 2, dir, out hit, range, SpotLayer)) {
            if (Vector3.Distance(HandTransform.position, hit.point) > minDistance) {
                foundspot = true;

                FindSpot(hit, sort);
            }
        }

        if (!foundspot)
        {
            if (Physics.Raycast(Spotlocation + transform.right * smallestEdge / 2, dir, out hit, range, SpotLayer))
            {
                if (Vector3.Distance(HandTransform.position, hit.point) > minDistance)
                {
                    foundspot = true;

                    FindSpot(hit, sort);
                }
            }
        }

        if (!foundspot) {
            if (Physics.Raycast(Spotlocation + transform.right * smallestEdge / 2 + transform.forward * smallestEdge, dir, out hit, range, SpotLayer))
            {
                if (Vector3.Distance(HandTransform.position, hit.point) - smallestEdge / 1.5f > minDistance)
                {
                    foundspot = true;

                    FindSpot(hit, sort);
                }
            }
        }

        if (!foundspot)
        {
            if (Physics.Raycast(Spotlocation - transform.right * smallestEdge / 2 + transform.forward * smallestEdge, dir, out hit, range, SpotLayer))
            {
                if (Vector3.Distance(HandTransform.position, hit.point) - smallestEdge / 1.5f > minDistance)
                {
                    foundspot = true;

                    FindSpot(hit, sort);
                }
            }
        }
    }

    public void FindSpot(RaycastHit h, Checkingsort sort) {
        if (Vector3.Angle(h.normal, Vector3.up) < MaxAngle) {
            RayInfo ray = new RayInfo();

            if (sort == Checkingsort.normal)
                ray = GetClosestPoint(h.transform, h.point + new Vector3(0.0f, -0.01f, 0.0f), transform.forward / 2.5f);
            else if (sort == Checkingsort.turning)
                ray = GetClosestPoint(h.transform, h.point + new Vector3(0.0f, -0.01f, 0.0f), transform.forward / 2.5f - transform.right * Input.GetAxis("Horizontal"));
            else if (sort == Checkingsort.falling)
                ray = GetClosestPoint(h.transform, h.point + new Vector3(0.0f, -0.01f, 0.0f), -transform.forward / 2.5f);

            TargetPoint = ray.point;
            TargetNormal = ray.normal;

            if(ray.CanGoToPoint) {
                if (currentSort != Climbingsort.Climbing && currentSort != Climbingsort.ClimbingTowardsPoint) {
                    TPUC.enabled = false;
                    rb.isKinematic = true;
                    TPC.m_IsGrounded = false;
                }

                currentSort = Climbingsort.ClimbingTowardsPoint;

                BeginDistance = Vector3.Distance(transform.position, (TargetPoint - transform.rotation * HandTransform.localPosition));
            }
        }
    }

    public RayInfo GetClosestPoint(Transform trans, Vector3 pos, Vector3 dir) {
        RayInfo curray = new RayInfo();
        RaycastHit hit2;

        int oldlayer = trans.gameObject.layer;

        //Change this
        trans.gameObject.layer = 14;

        if(Physics.Raycast(pos - dir, dir, out hit2, dir.magnitude * 2, CurrentSpotLayer)) {
            curray.point = hit2.point;
            curray.normal = hit2.normal;

            if (!Physics.Linecast(HandTransform.position + transform.rotation * new Vector3(0.0f, 0.05f, -0.05f), curray.point + new Vector3(0.0f, 0.5f, 0.0f), out hit2, CheckLayersReachable))
            {
                if (!Physics.Linecast(curray.point - Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f)) * curray.normal * 0.35f + 0.1f * curray.normal, curray.point + Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f)) * curray.normal * 0.35f + 0.1f * curray.normal, out hit2, CheckLayersForObstacle))
                {
                    if (!Physics.Linecast(curray.point + Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f)) * curray.normal * 0.35f + 0.1f * curray.normal, curray.point - Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f)) * curray.normal * 0.35f + 0.1f * curray.normal, out hit2, CheckLayersForObstacle))
                    {
                        curray.CanGoToPoint = true;
                    } else {
                        curray.CanGoToPoint = false;
                    }
                } else {
                    curray.CanGoToPoint = false;
                }
            } else {
                curray.CanGoToPoint = false;
            }

            trans.gameObject.layer = oldlayer;
            return curray;

        } else {
            trans.gameObject.layer = oldlayer;
            return curray;
        }
    }

    public void MoveTowardsPoint() {
        transform.position = Vector3.Lerp(transform.position, (TargetPoint - transform.rotation * HandTransform.localPosition), Time.deltaTime * ClimbForce);

        Quaternion lookrotation = Quaternion.LookRotation(-TargetNormal);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookrotation, Time.deltaTime * ClimbForce);

        anim.SetBool("OnGround", false);

        float distance = Vector3.Distance(transform.position, (TargetPoint - transform.rotation * HandTransform.localPosition));
        float percent = -9 * (BeginDistance - distance) / BeginDistance;

        anim.SetFloat("Jump", percent);

        if (distance <= 0.01f && currentSort == Climbingsort.ClimbingTowardsPoint) {
            transform.position = TargetPoint - transform.rotation * HandTransform.localPosition;
            transform.rotation = lookrotation;

            lasttime = Time.time;
            currentSort = Climbingsort.Climbing;
        }

        if (distance <= 0.25f && currentSort == Climbingsort.ClimbingTowardsPlateau)
        {
            transform.position = TargetPoint - transform.rotation * HandTransform.localPosition;
            transform.rotation = lookrotation;

            lasttime = Time.time;
            currentSort = Climbingsort.Walking;

            rb.isKinematic = false;
            TPUC.enabled = true;

        }
    }

    public void CheckForClimbStart() {
        RaycastHit hit2;
        
        Vector3 dir = transform.forward - transform.up / 0.8f;

        if (!Physics.Raycast(transform.position + transform.rotation * RaycastPosition, dir, 1.6f) && !Input.GetButton("Jump")) {
            currentSort = Climbingsort.CheckingForClimbStart;
            if (Physics.Raycast(transform.position + new Vector3(0.0f, 1.1f, 0.0f), -transform.up, out hit2, 1.6f, SpotLayer))
                FindSpot(hit2, Checkingsort.falling);
        }
    }

    public void CheckForPlateau() {
        RaycastHit hit2;

        Vector3 dir = transform.up + transform.forward / 2.0f;

        if (!Physics.Raycast(HandTransform.position + transform.rotation * VerticalHandOffset, dir, out hit2, 1.5f, SpotLayer)) {
            currentSort = Climbingsort.ClimbingTowardsPlateau;

            if (Physics.Raycast(HandTransform.position + dir * 1.5f, -Vector3.up, out hit2, 1.7f, SpotLayer))
                TargetPoint = HandTransform.position + dir * 1.5f;
            else
                TargetPoint = HandTransform.position + dir * 1.5f - transform.rotation * new Vector3(0, -0.2f, 0.25f);

            TargetNormal = -transform.forward;

            anim.SetBool("Crouch", true);
            anim.SetBool("OnGround", true);
        }
    }

    [System.Serializable]
    public enum Climbingsort {
        Walking,
        Jumping,
        Falling,
        Climbing,
        ClimbingTowardsPoint,
        ClimbingTowardsPlateau,
        CheckingForClimbStart
    }

    [System.Serializable]
    public class RayInfo {
        public Vector3 point;
        public Vector3 normal;
        public bool CanGoToPoint;
    }

    [System.Serializable]
    public enum Checkingsort
    {
        normal,
        turning,
        falling
    }
}
