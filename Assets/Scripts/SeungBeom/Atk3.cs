using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk3 : MonoBehaviour
{
    public bool Grow;
    public bool Shrink;
    public GameObject Explosion;

    SphereCollider sphere;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Growing());

        /*
        sphere = GetComponent<SphereCollider>();

        sphere.enabled = false;
        */
    }

    // Update is called once per frame
    void Update()
    {
        
        //-------------- 气惯 惯积 傈 付过柳 积己 -------------------
        if (Grow)
        {
            transform.localScale += new Vector3(100 * Time.deltaTime, 100 * Time.deltaTime, 100 * Time.deltaTime);
        }
        if (Shrink)
        {
            transform.localScale -= new Vector3(100 * Time.deltaTime, 100 * Time.deltaTime, 100* Time.deltaTime);
        }
    }
        //-------------- 气惯 惯积 傈 付过柳 积己 -------------------
    IEnumerator Growing()
    {

        Grow = true;
        yield return new WaitForSeconds(2f);
        Grow = false;

        yield return new WaitForSeconds(3f);
        Instantiate(Explosion, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(5f);    //气惯窃

        Shrink = true;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
