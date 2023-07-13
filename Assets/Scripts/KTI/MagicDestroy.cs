using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDestroy : MonoBehaviour
{
    public float Count;
    void Start()
    {
        Invoke("DestroyMagic", Count);
    }

    void DestroyMagic()
    {
        Destroy(gameObject);
    }
}
