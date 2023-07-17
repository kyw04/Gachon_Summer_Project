using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HookedTarget
{
    public Transform targetObj;
    public Vector3 hitPoint;
    public Vector3 normal;
    public float distance;
    public float mass;

    public HookedTarget(Transform _target, Vector3 _hitPoint, Vector3 _normal, float _distance, float _mass)
    {
        this.targetObj = _target;
        this.hitPoint = _hitPoint;
        this.normal = _normal;
        this.distance = _distance;
        this.mass = _mass;
    }
}

public class Hook : MonoBehaviour
{
    public Transform hookHead;
    public Transform firePosition;
    public float power;
    public float speed;
    public float range;
    public float playerHeight;

    private SpringJoint joint;
    private Transform movingObject;
    private Rigidbody playerRb;
    private Rigidbody rb;
    private HookedTarget hookedTarget;
    private Vector3 direction;
    private Vector3 destination;
    private float lastDistance;
    private float movementSpeed;
    private int mask;
    public bool isBack;
    public bool isMove;
    public bool isFixed;
    public bool isHold;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        firePosition = GameObject.FindWithTag("Hook").transform;
        firePosition.parent = this.transform;

        firePosition.position = new Vector3(0, 1, 0);
        hookHead = firePosition.GetChild(0);
        
        mask = ~LayerMask.GetMask("Player");

        isBack = false;
        isMove = false;
        isFixed = false;
        isHold = false;
    }

    public void HookControl(bool state)
    {
        //state = true == Fire1
        //state = false == Fire2 
        if (!isMove)
        {
            if (isFixed && hookedTarget.mass < power && state)
            {
                //Debug.Log($"Fire1 mass = {hookedTarget.mass}");
                isBack = true;
                Retract(firePosition, hookedTarget.targetObj);
            }
            else if (!state)
            {
                if (isFixed)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        Detach();
                    }
                    else
                    {
                        Retract(hookHead, transform);
                    }
                }
                else
                {
                    Fire();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (movingObject)
        {
            rb = movingObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = movingObject.GetComponentInChildren<Rigidbody>();
            }
        }

        if (isBack)
        {
            destination = firePosition.position;
            direction = (destination - hookHead.position).normalized;
        }

        if (isFixed)
        {
            RaycastHit hit;
            Vector3 dir = (hookHead.position - firePosition.position).normalized;
            float dis = Vector3.Distance(hookHead.position, firePosition.position);
            if (Physics.Raycast(firePosition.position, dir, out hit, dis, mask))
            {
                if (hit.collider.gameObject != hookedTarget.targetObj.gameObject)
                {
                    Debug.Log($"hit: {hit.collider.name}, hooked: {hookedTarget.targetObj.name}");
                    Detach(rb);
                }
            }

            if (!isMove)
                destination = hookHead.position;
            if (Physics.Raycast(destination, Vector3.up, playerHeight, mask))
            {
                destination.y -= playerHeight;
            }
        }
        else
        {
            playerRb.useGravity = true;
        }

        if (isMove)
        {
            bool isMoving = Move(movingObject, destination, direction, rb);
            if (!isMoving)
            {
                isMove = false;

                if (isBack || isFixed)
                {
                    isBack = false;
                    isFixed = false;
                    hookHead.position = firePosition.position;
                    Destroy(joint);

                    if (hookedTarget.targetObj)
                        hookHead.parent = null;
                    hookHead.parent = firePosition;
                }
                else if (hookedTarget.targetObj == null)
                {
                    Detach(rb);
                }
                else
                {
                    isFixed = true;
                    hookHead.parent = hookedTarget.targetObj;
                }
            }
        }
    }

    private bool Move(Transform moveObj, Vector3 des, Vector3 dir, Rigidbody rb)
    {
        if (rb)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }

        float dis = Vector3.Distance(moveObj.position + moveObj.localScale, des);
        if (isHold || dis > lastDistance)
        {
            //Debug.Log($"{dis} > {lastDistance}");
            if (!isBack)
                moveObj.position = des;

            if (rb)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                if (Input.GetButton("Fire2") && moveObj == transform) // hold
                {
                    moveObj.position = des;
                    isHold = true;
                    rb.useGravity = false;
                    return true;
                }
                isHold = false;
                rb.useGravity = true;
            }
            return false;
        }
        //hookHead.position = Vector3.Lerp(hookHead.position, direction, Time.deltaTime * movementSpeed);
        moveObj.position += dir * movementSpeed * Time.deltaTime;
        lastDistance = dis;

        return true;
    }
    private void Fire()
    {
        if (isMove || isFixed)
            return;

        //Debug.Log("Fire");
        isBack = false;
        isMove = true;
        hookHead.parent = null;
        hookHead.position = firePosition.position;

        Transform mainCamera = Camera.main.transform;
        Vector3 point = mainCamera.position + mainCamera.forward * range;

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, range, mask)) // find camera destination
        {
            point = hit.point + hit.normal * hookHead.localScale.y;

            hookedTarget = new HookedTarget(hit.collider.transform, point, hit.normal, hit.distance, float.PositiveInfinity);
            if (hit.collider.GetComponent<Rigidbody>())
            {
                hookedTarget.mass = hit.collider.GetComponent<Rigidbody>().mass;
            }
            //Debug.Log($"mass : {hookedTarget.mass}");
        }
        else
        {
            hookedTarget = new HookedTarget(null, point, Vector3.zero, range, float.PositiveInfinity);
        }

        joint = gameObject.AddComponent<SpringJoint>();
        joint.connectedAnchor = point;

        float distanceFromPoint = Vector3.Distance(firePosition.position, point);
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        movementSpeed = speed;
        SetDirection(hookHead, firePosition.position, point);
    }
    private void Retract(Transform pullingObj, Transform pulledTarget)
    {
        if (isMove)
            return;

        //Debug.Log("Retract");
        isMove = true;

        SetDirection(pulledTarget, pulledTarget.position, pullingObj.position);
        //Debug.Log($"{Vector3.Distance(destination, movingObject.position)} < {lastDistance}");
        if (isBack)
        {
            movementSpeed = (power - hookedTarget.mass) / power * speed;
        }
    }
    private void Detach(Rigidbody rb)
    {
        if (rb)
            rb.useGravity = true;
        Detach();
    }
    private void Detach()
    {
        if (destination == firePosition.position)
            return;

        //Debug.Log("Detach");
        isFixed = false;
        isBack = true;
        isMove = true;
        hookHead.parent = null;

        if (hookedTarget.targetObj)
        {
            hookHead.parent = null;
            hookedTarget.targetObj = null;
        }

        SetDirection(hookHead, hookHead.position, firePosition.position);
    }
    private void SetDirection(Transform moveObj, Vector3 startPos, Vector3 endPos) // real destination setting
    {
        //Debug.Log("set dir");
        movingObject = moveObj;
        direction = (endPos - startPos).normalized;
        lastDistance = float.PositiveInfinity;
        Vector3 des = endPos;

        RaycastHit hit;
        if (!isBack && Physics.Raycast(firePosition.position, direction, out hit, Vector3.Distance(startPos, endPos), mask))
        {
            des = hit.point + hit.normal * hookHead.localScale.y;

            hookedTarget = new HookedTarget(hit.collider.transform, des, hit.normal, hit.distance, float.PositiveInfinity);
            if (hit.collider.GetComponent<Rigidbody>())
            {
                hookedTarget.mass = hit.collider.GetComponent<Rigidbody>().mass;
            }
        }

        destination = des;
    }

    public bool IsUse()
    {
        return joint != null;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Transform mainCamera = Camera.main.transform;
        //Gizmos.DrawRay(mainCamera.position, mainCamera.forward * range);

        //Gizmos.color = Color.red;
        //Gizmos.DrawRay(firePosition.position, direction * range);

        //Gizmos.color = Color.green;
        //Vector3 dir = (hookHead.position - firePosition.position).normalized;
        //float dis = Vector3.Distance(hookHead.position, firePosition.position);
        //Gizmos.DrawRay(firePosition.position, dir * dis);
        //Gizmos.DrawSphere(hookHead.position, 0.25f);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawRay(hookHead.position, Vector3.up * playerHeight);
    }
}