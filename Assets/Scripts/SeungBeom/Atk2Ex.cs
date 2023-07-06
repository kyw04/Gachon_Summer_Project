using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk2Ex : MonoBehaviour
{
    public GameObject Circle;
    public GameObject Meteor;

    public bool Grow;
    public bool Shrink;
    // Start is called before the first frame update
    
    // Start is called before the first frame update
    void Start()
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

            Circle.transform.localScale += new Vector3(2 * 91 * Time.deltaTime, 2 * 91 * Time.deltaTime, 2 * 91 * Time.deltaTime);
        }
        if (Shrink)
        {
            Circle.transform.localScale -= new Vector3(3 * 91 * Time.deltaTime, 3 * 91 * Time.deltaTime, 3 * 91 * Time.deltaTime);
        }
    }
    IEnumerator Growing()
    {

        Grow = true;
        yield return new WaitForSeconds(0.5f);
        Grow = false;

        yield return new WaitForSeconds(2.9f);  //3초 후에 메테오 생성.
        Meteor.SetActive(true);
        Shrink = true;
        yield return new WaitForSeconds(1f);
        Shrink = false;
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}
