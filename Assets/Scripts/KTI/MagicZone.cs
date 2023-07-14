using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicZone : MonoBehaviour
{
    public GameObject Door;
    public float Time;
    public GameObject Spawner;
    public GameObject Firecool;
    public GameObject Groundcool;
    public GameObject Magiccool;
    public GameObject Text1;
    public GameObject Text2;
    public GameObject Boss;

    void Start()
    {
        Door.SetActive(true);
        Spawner.SetActive(false);
        Firecool.SetActive(false);
        Groundcool.SetActive(false);
        Magiccool.SetActive(false);
        Text1.SetActive(false); 
        Text2.SetActive(false);
        Boss.SetActive(false);  
    }
    

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            Door.SetActive(false);
            StartCoroutine("Maintain");
            Spawner.SetActive(true);
            Firecool.SetActive(true);
            Groundcool.SetActive(true);
            Magiccool.SetActive(true);
            StartCoroutine("Text");
            Boss.SetActive(true);
            
        }
    }

    IEnumerator Maintain()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            Door.SetActive(true);
            break;
        }
    }

    IEnumerator Text()
    {
        while (true)
        {
            Text1.SetActive(true); 
            yield return new WaitForSeconds(2f);
            Text1.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            Text2.SetActive(true);
            yield return new WaitForSeconds(2f);
            Text2.SetActive(false);
            break;
        }
    }
}
