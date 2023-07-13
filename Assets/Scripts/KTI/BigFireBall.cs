using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class BigFireBall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Bottom")
        {
            Destroy(gameObject);;
        }
        if (collision.gameObject.tag == "Fire")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
       
    }
}
