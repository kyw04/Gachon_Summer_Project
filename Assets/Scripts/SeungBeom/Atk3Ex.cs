using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk3Ex : MonoBehaviour
{
    public GameObject Atk3Circle;
    public GameObject Atk3Explosion;

    public bool Grow;
    public bool Shrink;

    // Start is called before the first frame update
    void Start()
    {
        Atk3Circle = transform.GetChild(0).gameObject;
        Atk3Explosion = transform.GetChild(1).gameObject;

        Atk3Explosion.SetActive(false);

        StartCoroutine(Growing());
    }

    // Update is called once per frame
    void Update()
    {
        if (Grow)
        {
            Atk3Circle.transform.localScale += new Vector3(100 * Time.deltaTime, 100 * Time.deltaTime, 100 * Time.deltaTime);
        }
        if (Shrink)
        {
            Atk3Circle.transform.localScale -= new Vector3(100 * Time.deltaTime, 100 * Time.deltaTime, 100 * Time.deltaTime);
        }
    }
    IEnumerator Growing()
    {

        Grow = true;
        yield return new WaitForSeconds(2f);
        Grow = false;

        yield return new WaitForSeconds(3f);    //5초후에 폭발을 생성해야 합니다.
        Atk3Explosion.SetActive(true);

        yield return new WaitForSeconds(5f);    //폭발함

        Shrink = true;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
