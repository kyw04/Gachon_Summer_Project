using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Bullet :MonoBehaviour
{ 
    public int damdge;
    public bool isBall;

    BossHPViewer boss;
   
    private void Start()
    {
        boss = GameObject.Find("BossHPSlider").GetComponent<BossHPViewer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!isBall && collision.gameObject.tag == "Floor") 
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
      
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
            boss.Fire();
        }
       
        
    }
}

   
 

