using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk5Small : MonoBehaviour
{
    GameObject Meteor;
    GameObject explo;

    GameObject MagicCircle;

    bool Grow;
    bool Shrink;
    // Start is called before the first frame update
    private void Awake()
    {
        Meteor = transform.GetChild(0).gameObject;
        explo = transform.GetChild(1).gameObject;
        MagicCircle = transform.GetChild(2).gameObject;
        Meteor.SetActive(false);
        explo.SetActive(false);
        MagicCircle.SetActive(false);
    }
    void OnEnable()
    {
        Meteor.SetActive(false);
        explo.SetActive(false);
        MagicCircle.SetActive(false);
        StartCoroutine(Ex());
    }
    private void Update()
    {
        if (Grow)
        {
            MagicCircle.transform.localScale += new Vector3(42 * Time.deltaTime, 42 * Time.deltaTime, 42 * Time.deltaTime);
        }
        if (Shrink)
        {
            MagicCircle.transform.localScale -= new Vector3(21 * Time.deltaTime, 21 * Time.deltaTime, 21 * Time.deltaTime);
        }
    }

    IEnumerator Ex()
    {
        yield return new WaitForSeconds(0.1f);
        MagicCircle.SetActive(true);
        Grow = true;
        yield return new WaitForSeconds(0.5f);
        Grow = false;
        yield return new WaitForSeconds(3.5f);
        Meteor.SetActive(true);
        Shrink = true;
        yield return new WaitForSeconds(0.7f);
        explo.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        Shrink = false;
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
    }
}
