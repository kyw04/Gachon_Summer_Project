using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk3Ex : MonoBehaviour
{
    bool Grow;
    bool Shrink;
    public GameObject Circle;
    public GameObject Explosion;
    // Start is called before the first frame update
    void OnEnable()
    {
        Circle = transform.GetChild(0).gameObject;
        Explosion = transform.GetChild(1).gameObject;

        Explosion.SetActive(false);
        Circle.SetActive(true);

        StartCoroutine(Growing());
    }

    // Update is called once per frame
    void Update()
    {
        if (Grow)
        {
            Circle.transform.localScale += new Vector3(100 * Time.deltaTime, 100 * Time.deltaTime, 100 * Time.deltaTime);
        }
        if (Shrink)
        {
            Circle.transform.localScale -= new Vector3(100 * Time.deltaTime, 100 * Time.deltaTime, 100 * Time.deltaTime);
        }
    }
    IEnumerator Growing()
    {

        Grow = true;
        yield return new WaitForSeconds(2f);
        Grow = false;

        yield return new WaitForSeconds(3f);
        Explosion.SetActive(true);

        yield return new WaitForSeconds(5f);    //Æø¹ßÇÔ
        
        Shrink = true;
        yield return new WaitForSeconds(2f);
        Shrink = false;
        Circle.transform.localScale = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
    }
}
