using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HookedTarget
{
    public GameObject targetObj;
    public Vector3 hitPoint;
    public float distance;
    public float weight;

    public HookedTarget(GameObject target)
    {
        this.targetObj = target;
        hitPoint = Vector3.zero;
        distance = 0f;
        weight = 0f;
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
    private bool isHookFixed;

    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Fire();
        }

        if (hookedTarget.targetObj)
        {

        }
    }

    public void Fire()
    {
        if (isHookFixed)
            return;
        hookHead.position = firePosition.position;

        Transform mainCamera = Camera.main.transform;
        Vector3 dir = mainCamera.forward * range;

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, range))
        {
            Debug.Log(hit.collider.name);
            dir = hit.point;
        }
        hookHead.position = dir;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Transform mainCamera = Camera.main.transform;
        Gizmos.DrawRay(mainCamera.position, mainCamera.forward * range);
    }
}
