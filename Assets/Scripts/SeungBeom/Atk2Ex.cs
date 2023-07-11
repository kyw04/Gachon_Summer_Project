using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk2Ex : MonoBehaviour
{

    public GameObject Circle;
    public GameObject Meteor;


    bool Grow;
    bool Shrink;
    // Start is called before the first frame update
    void OnEnable()
    {
        Circle = transform.GetChild(0).gameObject;
        Meteor = transform.GetChild(1).gameObject;

        Meteor.SetActive(false);
        Circle.SetActive(true);

        StartCoroutine(Growing());

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Grow)
        {
            Circle.transform.localScale += new Vector3(2 * 22.5f * Time.deltaTime, 2 * 22.5f * Time.deltaTime, 2 * 22.5f * Time.deltaTime);
        }
        if (Shrink)
        {
            Circle.transform.localScale -= new Vector3(2 * 22.5f * Time.deltaTime, 2 * 22.5f * Time.deltaTime, 2 * 22.5f * Time.deltaTime);
        }
    }
    IEnumerator Growing()
    {

        Grow = true;
        yield return new WaitForSeconds(0.5f);
        Grow = false;

        yield return new WaitForSeconds(2.5f);  //3 초 후에 폭발 발생
        Meteor.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        Shrink = true;
        
        yield return new WaitForSeconds(0.6f);
        Shrink = false;
        Circle.transform.localScale = new Vector3(1, 1, 1);
        Circle.SetActive(false);
        
        yield return new WaitForSeconds(2.9f);
        gameObject.SetActive(false);
    }
}
