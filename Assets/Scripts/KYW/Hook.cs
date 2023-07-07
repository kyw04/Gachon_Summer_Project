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

public class Hook : MonoBehaviour
{
    public Transform hookHead;
    public Transform firePosition;
    public float power;
    public float speed;
    public float range;

    private Transform movingObject;
    private HookedTarget hookedTarget;
    private Vector3 direction;
    private Vector3 destination;
    private float lastDistance;
    private float movementSpeed;
    public bool isBack;
    public bool isMove;
    public bool isFixed;

    private void Start()
    {
        isBack = false;
        isMove = false;
        isFixed = false;
    }

    private void Update()
    {
        if (isFixed && Input.GetButtonDown("Fire1"))
        {
            Retract(hookHead, this.transform);
        }
        //if  (Input.GetButtonUp("Fire1"))
        //{
        //    rigi.useGravity = true;
        //}

        if (Input.GetButtonDown("Fire2"))
        {
            if (isFixed)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Detach();
                }
                else if (hookedTarget.mass != -1 && hookedTarget.mass < power)
                {
                    isBack = true;
                    Retract(transform, hookHead);
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
            bool isMoving = Move(movingObject, destination, direction, movingObject.GetComponent<Rigidbody>());
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
    }

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
                rb.useGravity = true;

                if (Input.GetButton("Fire1"))
                {
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;

                    //Move(moveObj, des, dir, rb);
                    return true;
                }
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
        movingObject = hookHead;

        Transform mainCamera = Camera.main.transform;
        Vector3 point = mainCamera.position + mainCamera.forward * range;

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, range))
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
            hookedTarget = new HookedTarget(null, point, Vector3.zero, range, 0f);
        }

        movementSpeed = speed;
        direction = (point - firePosition.position).normalized;
        destination = point;
        lastDistance = range + 1f;
        //hookHead.position = dir;
    }

    public void Retract(Transform pullingObj, Transform pulledTarget)
    {
        isMove = true;

        movingObject = pulledTarget;
        direction = (pullingObj.position - pulledTarget.position).normalized;
        destination = pullingObj.position;
        lastDistance = range + 1f;

        if (hookedTarget.mass != -1 && isBack)
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
        movingObject = hookHead;

        if (hookedTarget.targetObj)
        {
            hookedTarget.targetObj.transform.parent = null;
            hookedTarget.targetObj = null;
        }

        direction = (firePosition.position - hookedTarget.hitPoint).normalized;
        hookHead.position += direction * movementSpeed * Time.deltaTime;
        destination = firePosition.position;
        lastDistance = range + 1f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Transform mainCamera = Camera.main.transform;
        Gizmos.DrawRay(mainCamera.position, mainCamera.forward * range);
    }
}
