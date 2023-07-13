using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk3Ex : MonoBehaviour
{
    bool Grow;
    bool Shrink;
    public GameObject Circle;
    public GameObject Explosion;
    bool LightOn;
    Light ligh;
    // Start is called before the first frame update
    void OnEnable()
    {
        Circle = transform.GetChild(0).gameObject;
        Explosion = transform.GetChild(1).gameObject;
        ligh = GetComponent<Light>();

        Explosion.SetActive(false);
        Circle.SetActive(true);

        StartCoroutine(Growing());
    }

    // Update is called once per frame
    void Update()
    {
        if (Grow)
        {
            Circle.transform.localScale += new Vector3(25 * Time.deltaTime, 25 * Time.deltaTime, 25 * Time.deltaTime);
        }
        if (Shrink)
        {
            Circle.transform.localScale -= new Vector3(25 * Time.deltaTime, 25 * Time.deltaTime, 25 * Time.deltaTime);
        }
        if(LightOn)
        {
            ligh.range += 50 * Time.deltaTime;
            ligh.intensity += 0.5f * Time.deltaTime;
        }
    }
    IEnumerator Growing()
    {
        ligh.enabled = true;
        Grow = true;
        yield return new WaitForSeconds(2f);
        Grow = false;

        yield return new WaitForSeconds(3f);
        Explosion.SetActive(true);
        LightOn = true;
        yield return new WaitForSeconds(5f);    //Æø¹ßÇÔ
        LightOn = false;
        ligh.range = 0;
        ligh.intensity = 1;
        ligh.enabled = false;
        Shrink = true;
        yield return new WaitForSeconds(2f);
        Shrink = false;
        Circle.transform.localScale = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
    }
}
