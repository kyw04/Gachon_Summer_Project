using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    public GameObject Spawner;
    public GameObject Firecool;
    public GameObject Groundcool;
    public GameObject Magiccool;
    public GameObject Text1;
    public GameObject Text2;
    public GameObject Boss;
    int Onestart;

    void Start()
    {
        Spawner.SetActive(false);
        Firecool.SetActive(false);
        Groundcool.SetActive(false);
        Magiccool.SetActive(false);
        Text1.SetActive(false);
        Text2.SetActive(false);
        Boss.SetActive(false);
        Onestart = 0;
    }

  void Update ()
    {

        if (Input.GetKeyDown(KeyCode.F1) && Onestart == 0)
        {
            Spawner.SetActive(true);
            Firecool.SetActive(true);
            Groundcool.SetActive(true);
            Magiccool.SetActive(true);
            StartCoroutine("Text");
            Boss.SetActive(true);
            Onestart += 1;
        }
    }

    IEnumerator Text()
    {
        Text1.SetActive(true);
        yield return new WaitForSeconds(2f);
        Text1.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Text2.SetActive(true);
        yield return new WaitForSeconds(2f);
        Text2.SetActive(false);
    }
}


