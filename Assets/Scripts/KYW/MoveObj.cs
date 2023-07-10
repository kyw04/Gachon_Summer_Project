using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObj : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotateDelay = 5f;

    private float lastRotate;

    void Start()
    {
        lastRotate = 0;
    }

    void Update()
    {
        if (lastRotate + rotateDelay <= Time.time)
        {
            lastRotate = Time.time;
            movementSpeed = -movementSpeed;
        }
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }
}
