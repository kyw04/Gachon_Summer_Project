using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HookedTarget
{
    public GameObject targetObj;
    public Vector3 hitPoint;
    public float distance;
    public float weight;
}

public class Hook : MonoBehaviour
{
    public Transform firePosition;
    public float power;
    public float speed;
    public float range;

    private HookedTarget hookedTarget;
    private bool isHookFixed;


}
