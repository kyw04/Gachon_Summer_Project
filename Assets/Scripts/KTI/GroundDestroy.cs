using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDestroy : MonoBehaviour
{
    public float Count;
    void Start()
    {
        Invoke("DestroyGround", Count);
    }

    void DestroyGround()
    {
        Destroy(gameObject);
    }
}
