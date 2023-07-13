using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk5Small : MonoBehaviour
{
    GameObject Meteor;
    GameObject explo;
    // Start is called before the first frame update
    private void Awake()
    {
        Meteor = transform.GetChild(0).gameObject;
        explo = transform.GetChild(1).gameObject;
        Meteor.SetActive(false);
        explo.SetActive(false);
    }
    void OnEnable()
    {
        StartCoroutine(Ex());
    }

    IEnumerator Ex()
    {
        yield return new WaitForSeconds(0.1f);
        Meteor.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        explo.SetActive(true);
        yield return new WaitForSeconds(4f);
        Meteor.SetActive(false);
        explo.SetActive(false);
    }
}
