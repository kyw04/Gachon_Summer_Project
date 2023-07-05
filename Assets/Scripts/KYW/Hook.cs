using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HookedTarget
{
    public GameObject targetObj;
    public Vector3 hitPoint;
    public float distance;
    public float weight;

    public HookedTarget(GameObject _target, Vector3 _hitPoint, float _distance, float _weight)
    {
        this.targetObj = _target;
        this.hitPoint = _hitPoint;
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

    private HookedTarget hookedTarget;
    private Vector3 direction;
    private Vector3 destination;
    private Vector3 startingPos;
    public bool isMove;
    public bool isFixed;

    private void Start()
    {
        isMove = false;
        isFixed = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (isFixed)
                Detach();
            else
                Fire();
        }

        if (isMove)
        {
            Debug.Log(Vector3.Distance(hookHead.position, startingPos));
            //Debug.Log(hookedTarget.distance);
            //Debug.Log(Vector3.Distance(hookHead.position, direction).ToString("F1"));
            if (Vector3.Distance(hookHead.position, startingPos) >= hookedTarget.distance)
            //if (hookHead.position.magnitude >= destination.magnitude)
            {
                isMove = false;
                if (hookedTarget.targetObj == null)
                {
                    Detach();
                }
                else
                {
                    isFixed = true;
                }
            }
            else
            {
                //hookHead.position = Vector3.Lerp(hookHead.position, direction, Time.deltaTime * speed);
                hookHead.position += direction * speed * Time.deltaTime;
            }
        }
    }

    public void Fire()
    {
        if (isMove)
            return;

        isMove = true;
        hookHead.position = firePosition.position;

        Transform mainCamera = Camera.main.transform;
        Vector3 point = mainCamera.position + mainCamera.forward * range;

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, range))
        {
            point = hit.point;

            hookedTarget = new HookedTarget(hit.collider.gameObject, point, hit.distance, -1f);

            if (hit.collider.GetComponent<Rigidbody>())
            {
                hookedTarget.weight = hit.collider.GetComponent<Rigidbody>().mass;
            }
        }
        else
        {
            hookedTarget = new HookedTarget(null, point, range, 0f);
        }

        direction = (point - firePosition.position).normalized;
        destination = point;
        startingPos = firePosition.position;
        //hookHead.position = dir;
    }

    public void Retract()
    {

    }

    public void Detach()
    {
        if (destination == firePosition.position)
            return;

        isFixed = false;
        isMove = true;
        direction = (firePosition.position - hookedTarget.hitPoint).normalized;
        hookHead.position += direction * speed * Time.deltaTime;
        startingPos = destination;
        hookedTarget.targetObj = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Transform mainCamera = Camera.main.transform;
        Gizmos.DrawRay(mainCamera.position, mainCamera.forward * range);
    }
}
