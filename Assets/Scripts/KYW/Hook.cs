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

[RequireComponent(typeof(LineRenderer))]
public class Hook : MonoBehaviour
{
    public Transform hookHead;
    public Transform firePosition;
    public float power;
    public float speed;
    public float range;

    private LineRenderer chain;
    private Transform movingObject;
    private HookedTarget hookedTarget;
    private Vector3 direction;
    private Vector3 destination;
    private float lastDistance;
    private float movementSpeed;
    private int mask;
    public bool isBack;
    public bool isMove;
    public bool isFixed;

    private void Start()
    {
        firePosition.parent = GameObject.Find("Player").transform;
        mask = ~LayerMask.GetMask("Player");
        chain = GetComponent<LineRenderer>();
        isBack = false;
        isMove = false;
        isFixed = false;
    }

    private void Update()
    {
        if (!isMove)
        {
            if (isFixed && Input.GetButtonDown("Fire1"))
            {
                if (hookedTarget.mass < power)
                {
                    //Debug.Log($"Fire1 mass = {hookedTarget.mass}");
                    isBack = true;
                    Retract(firePosition, hookedTarget.targetObj);
                }
            }
            else if (Input.GetButtonDown("Fire2"))
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
        if (isBack)
        {
            destination = firePosition.position;
            direction = (destination - hookHead.position).normalized;
        }

        Rigidbody rb = null;
        if (movingObject)
        {
            rb = movingObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = movingObject.GetComponentInChildren<Rigidbody>();
            }
        }

        if (isMove)
        {
            bool isMoving = Move(movingObject, destination, direction, rb);
            if (!isMoving)
            {
                hookHead.DetachChildren();
                chain.enabled = false;
                isMove = false;
                //if (rb != null)
                //    rb.useGravity = true;

                if (isBack || isFixed)
                {
                    isBack = false;
                    isFixed = false;
                    hookHead.position = firePosition.position;

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
        }

        if (movingObject) // draw line
        {
            chain.enabled = true;
            Vector3[] chainPos = new Vector3[2];
            chainPos[0] = hookHead.position;
            chainPos[1] = firePosition.position;
            chain.SetPositions(chainPos);
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
        if (dis > lastDistance)
        {
            //Debug.Log(dis);
            if (!isBack)
                moveObj.position = des;

            if (rb)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                if (Input.GetButton("Fire2") && moveObj == transform) // hold
                {
                    //Debug.Log("Hold");
                    rb.useGravity = false;
                    return true;
                }
                rb.useGravity = true;
            }
            return false;
        }
        //hookHead.position = Vector3.Lerp(hookHead.position, direction, Time.deltaTime * movementSpeed);
        moveObj.position += dir * movementSpeed * Time.deltaTime;
        lastDistance = dis;

        return true;
    }
    public void Fire()
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
            point = hit.point + hit.normal * hookHead.localScale.y/* * 0.51f*/;

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

        movementSpeed = speed;
        SetDirection(hookHead, firePosition.position, point);
    }
    public void Retract(Transform pullingObj, Transform pulledTarget)
    {
        if (isMove)
            return;

        //Debug.Log("Retract");
        isMove = true;

        SetDirection(pulledTarget, pulledTarget.position, pullingObj.position);
        //Debug.Log($"Retract mass = {hookedTarget.mass}");
        if (isBack)
        {
            movementSpeed = (power - hookedTarget.mass) / power * speed;
        }

        //Debug.Log($"{lastDistance.ToString("F1")} {Vector3.Distance(movingObject.position, destination)}");
    }
    public void Detach(Rigidbody rb)
    {
        if (rb)
            rb.useGravity = true;
        Detach();
    }
    public void Detach()
    {
        if (destination == firePosition.position)
            return;

        //Debug.Log("Detach");
        isBack = true;
        isFixed = false;
        isMove = true;
        hookHead.parent = null;

        if (hookedTarget.targetObj)
        {
            hookHead.parent = null;
            hookedTarget.targetObj = null;
        }

        SetDirection(hookHead, hookedTarget.hitPoint, firePosition.position);
        hookHead.position += direction * movementSpeed * Time.deltaTime;
    }
    private void SetDirection(Transform moveObj, Vector3 startPos, Vector3 endPos) // real destination setting
    {
        movingObject = moveObj;
        direction = (endPos - startPos).normalized;
        lastDistance = float.PositiveInfinity;
        Vector3 des = endPos;

        Debug.DrawRay(firePosition.position, direction * Vector3.Distance(startPos, endPos), Color.red);
        RaycastHit hit;
        if (!isBack && Physics.Raycast(firePosition.position, direction, out hit, Vector3.Distance(startPos, endPos), mask))
        {
            //Debug.Log(hit.collider.name);
            des = hit.point + hit.normal * hookHead.localScale.y/* * 0.51f*/;

            hookedTarget = new HookedTarget(hit.collider.transform, des, hit.normal, hit.distance, float.PositiveInfinity);
            //Debug.Log(hit.distance);
            if (hit.collider.GetComponent<Rigidbody>())
            {
                hookedTarget.mass = hit.collider.GetComponent<Rigidbody>().mass;
            }
        }

        destination = des;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Transform mainCamera = Camera.main.transform;
        Gizmos.DrawRay(mainCamera.position, mainCamera.forward * range);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(firePosition.position, direction * range);

        Gizmos.color = Color.green;
        Vector3 dir = (hookHead.position - firePosition.position).normalized;
        float dis = Vector3.Distance(hookHead.position, firePosition.position);
        Gizmos.DrawRay(firePosition.position, dir * dis);
    }
}
