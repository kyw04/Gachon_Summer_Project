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
    private void Awake()
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
    // Start is called before the first frame update
    void Start()
    {
        eight.SetActive(false);

        one.SetActive(false);

        two.SetActive(false);
        
        three.SetActive(false);
        
        four.SetActive(false);
        
        five.SetActive(false);
       
        six.SetActive(false);

        seven.SetActive(false);

        StartCoroutine(Atk4pivot());
    }
    IEnumerator Atk4pivot()
    {
        while(true)
        {
            yield return new WaitForSeconds(2);
            transform.LookAt(new Vector3(TARGET.position.x, transform.position.y,TARGET.position.z));
            eight.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            one.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            two.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            three.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            four.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            five.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            six.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            seven.SetActive(true);
            yield return new WaitForSeconds(6);
        }
    }
}
