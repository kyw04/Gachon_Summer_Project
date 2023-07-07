using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HookedTarget
{
    public GameObject targetObj;
    public Vector3 hitPoint;
    public Vector3 normal;
    public float distance;
    public float mass;

    public HookedTarget(GameObject _target, Vector3 _hitPoint, Vector3 _normal, float _distance, float _mass)
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
        mask = ~LayerMask.GetMask("Player");
        chain = GetComponent<LineRenderer>();
        isBack = false;
        isMove = false;
        isFixed = false;
    }

    private void Update()
    {
        if (isFixed && Input.GetButtonDown("Fire1"))
        {
            if (hookedTarget.mass < power)
            {
                isBack = true;
                Retract(transform, hookHead);
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
                    Retract(hookHead, this.transform);
                }

            }
            else
            {
                Fire();
            }
        }

        if (isBack)
        {
            destination = firePosition.position;
            direction = (destination - hookHead.position).normalized;
        }

        if (isMove)
        {
            Rigidbody rb = movingObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = movingObject.GetComponentInChildren<Rigidbody>();
            }

            bool isMoving = Move(movingObject, destination, direction, rb);
            if (!isMoving)
            {
                isMove = false;
                if (isBack || isFixed)
                {
                    isBack = false;
                    isFixed = false;

                    if (hookedTarget.targetObj)
                        hookedTarget.targetObj.transform.parent = null;
                    hookHead.parent = firePosition;
                }
                else
                {
                    if (hookedTarget.targetObj == null)
                    {
                        Detach();
                    }
                    else
                    {
                        //hookHead.position = hookHead.position + hookedTarget.normal * hookHead.localScale.y * 0.5f;
                        //destination = hookHead.position;
                        isFixed = true;
                        hookedTarget.targetObj.transform.parent = hookHead;
                    }
                }
            }
        }

        if (movingObject)
        {
            Vector3[] chainPos = new Vector3[2];
            chainPos[0] = movingObject.position;
            chainPos[1] = movingObject == transform ? hookHead.position : transform.position;
            chain.SetPositions(chainPos);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (hookHead.childCount > 0 && collision.transform == hookHead.GetChild(0))
    //    {
    //        Detach();
    //    }
    //}

    private bool Move(Transform moveObj, Vector3 des, Vector3 dir, Rigidbody rb)
    {
        if (rb)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }

        float dis = Vector3.Distance(moveObj.position + moveObj.localScale, des);
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, moveObj.position - transform.position, out hit, dis + 0.1f))
        //{
        //    Debug.Log(hit.collider.name);
        //    des = hit.point + hit.normal * transform.localScale.x;
        //    //Debug.DrawRay(transform.position, moveObj.position - transform.position * (dis + 0.1f), Color.red);
        //}

        //Debug.Log(dis.ToString("F1") + " " + lastDistance.ToString("F1"));
        if (dis > lastDistance)
        {
            //Debug.Log(dis);
            moveObj.position = des;

            if (rb)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                if (Input.GetButton("Fire2"))
                {
                    rb.useGravity = false;

                    //Move(moveObj, des, dir, rb);
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

        isBack = false;
        isMove = true;
        hookHead.parent = null;
        hookHead.position = firePosition.position;

        Transform mainCamera = Camera.main.transform;
        Vector3 point = mainCamera.position + mainCamera.forward * range;

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, range, mask))
        {
            point = hit.point + hit.normal * hookHead.localScale.y * 0.51f;

            hookedTarget = new HookedTarget(hit.collider.gameObject, point, hit.normal, hit.distance, -1f);
            if (hit.collider.GetComponent<Rigidbody>())
            {
                hookedTarget.mass = hit.collider.GetComponent<Rigidbody>().mass;
            }
        }
        else
        {
            hookedTarget = new HookedTarget(null, point, Vector3.zero, range, float.PositiveInfinity);
        }

        movementSpeed = speed;
        SetDirection(hookHead, firePosition.position, point);
        //hookHead.position = dir;
    }

    public void Retract(Transform pullingObj, Transform pulledTarget)
    {
        if (isMove)
            return;

        isMove = true;

        SetDirection(pulledTarget, pulledTarget.position, pullingObj.position);

        if (isBack)
        {
            movementSpeed = (power - hookedTarget.mass) / power * movementSpeed;
        }

        //Debug.Log($"{lastDistance.ToString("F1")} {Vector3.Distance(movingObject.position, destination)}");
    }

    public void Detach()
    {
        if (destination == firePosition.position)
            return;
        //Debug.Log("detach");

        isBack = true;
        isFixed = false;
        isMove = true;
        hookHead.parent = null;

        if (hookedTarget.targetObj)
        {
            hookedTarget.targetObj.transform.parent = null;
            hookedTarget.targetObj = null;
        }

        SetDirection(hookHead, hookedTarget.hitPoint, firePosition.position);
        hookHead.position += direction * movementSpeed * Time.deltaTime;
    }

    private void SetDirection(Transform moveObj, Vector3 startPos, Vector3 endPos)
    {
        movingObject = moveObj;
        direction = (endPos - startPos).normalized;
        lastDistance = float.PositiveInfinity;
        Vector3 des = endPos;

        RaycastHit hit;
        if (Physics.Raycast(firePosition.position, direction, out hit, hookedTarget.distance, mask))
        {
            des = hit.point + hit.normal * hookHead.localScale.y * 0.51f;

            hookedTarget = new HookedTarget(hit.collider.gameObject, des, hit.normal, hit.distance, float.PositiveInfinity);
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
    }
}
