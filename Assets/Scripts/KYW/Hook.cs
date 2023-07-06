using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HookedTarget
{
    public GameObject targetObj;
    public Vector3 hitPoint;
    public Vector3 normal;
    public float distance;
    public float weight;

    public HookedTarget(GameObject _target, Vector3 _hitPoint, Vector3 _normal, float _distance, float _weight)
    {
        this.targetObj = _target;
        this.hitPoint = _hitPoint;
        this.normal = _normal;
        this.distance = _distance;
        this.weight = _weight;
    }
}

public class Hook : MonoBehaviour
{
    public Transform hookHead;
    public Transform firePosition;
    public float power;
    public float speed;
    public float range;

    private Rigidbody rigi;
    private Transform movingObject;
    private HookedTarget hookedTarget;
    private Vector3 direction;
    private Vector3 destination;
    private float lastDistance;
    public bool isBack;
    public bool isMove;
    public bool isFixed;

    private void Start()
    {
        rigi = GetComponent<Rigidbody>();

        isBack = false;
        isMove = false;
        isFixed = false;
    }

    private void Update()
    {
        if (isFixed && Input.GetButtonDown("Fire1"))
        {
            Retract();
        }
        //if  (Input.GetButtonUp("Fire1"))
        //{
        //    rigi.useGravity = true;
        //}
        
        if (Input.GetButtonDown("Fire2"))
        {
            if (isFixed)
                Detach();
            else
                Fire();
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

        //hookHead.position = Vector3.Lerp(hookHead.position, direction, Time.deltaTime * speed);
        moveObj.position += dir * speed * Time.deltaTime;
        lastDistance = dis;
        return true;
    }

    public void Fire()
    {
        if (isMove)
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
            
            //Debug.Log(hit.collider.contactOffset);

            hookedTarget = new HookedTarget(hit.collider.gameObject, point, hit.normal, hit.distance, -1f);

            if (hit.collider.GetComponent<Rigidbody>())
            {
                hookedTarget.weight = hit.collider.GetComponent<Rigidbody>().mass;
            }
        }
        else
        {
            hookedTarget = new HookedTarget(null, point, Vector3.zero, range, 0f);
        }

        direction = (point - firePosition.position).normalized;
        destination = point;
        lastDistance = range + 1f;
        //hookHead.position = dir;
    }

    public void Retract()
    {
        isMove = true;

        movingObject = this.transform;
        direction = (hookHead.position - movingObject.position).normalized;
        destination = hookHead.position;
        lastDistance = range + 1f;
        //Debug.Log($"{lastDistance.ToString("F1")} {Vector3.Distance(movingObject.position, destination)}");
    }

    public void Detach()
    {
        if (destination == firePosition.position)
            return;

        isBack = true;
        isFixed = false;
        isMove = true;
        hookHead.parent = null;
        movingObject = hookHead;

        direction = (firePosition.position - hookedTarget.hitPoint).normalized;
        hookHead.position += direction * speed * Time.deltaTime;
        destination = firePosition.position;
        hookedTarget.targetObj = null;
        lastDistance = range + 1f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Transform mainCamera = Camera.main.transform;
        Gizmos.DrawRay(mainCamera.position, mainCamera.forward * range);
    }
}
