using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var v = collision.gameObject.GetComponent<PlayerComponent>();
            v.ModifyHealthPoint(-20);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
