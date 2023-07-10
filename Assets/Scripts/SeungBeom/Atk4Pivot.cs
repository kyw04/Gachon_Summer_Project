using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk4Pivot : MonoBehaviour
{
    Transform TARGET;
    //Transform Targetposition;
    GameObject one;
    GameObject two;
    GameObject three;
    GameObject four;
    GameObject five;
    GameObject six;
    GameObject seven;
    GameObject eight;

    // Start is called before the first frame update
    void Start()
    {
        
        TARGET = GameObject.FindGameObjectWithTag("Player").transform;
        //Targetposition.position = new Vector3(TARGET.transform.position.x, 0, TARGET.transform.position.z);
        transform.LookAt(new Vector3(TARGET.position.x, transform.position.y, TARGET.position.z));
        StartCoroutine(Atk4pivot());
        one = transform.GetChild(0).gameObject;
        two = transform.GetChild(1).gameObject;
        three = transform.GetChild(2).gameObject;
        four = transform.GetChild(3).gameObject;
        five = transform.GetChild(4).gameObject;
        six = transform.GetChild(5).gameObject;
        seven = transform.GetChild(6).gameObject;
        eight = transform.GetChild(7).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Atk4pivot()
    {
        while(true)
        {
            
            yield return new WaitForSeconds(2);
            transform.LookAt(new Vector3(TARGET.position.x, transform.position.y,TARGET.position.z));
            eight.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            one.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            two.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            three.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            four.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            five.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            six.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            seven.SetActive(true);
            yield return new WaitForSeconds(6.6f);
        }
    }
}
