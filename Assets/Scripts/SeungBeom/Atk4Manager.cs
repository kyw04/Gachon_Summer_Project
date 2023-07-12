using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk4Manager : MonoBehaviour
{
    GameObject A41;
    GameObject A42;
    GameObject A43;
    GameObject A44;
    GameObject A45;
    // Start is called before the first frame update
    private void Awake()
    {
        A41 = transform.GetChild(0).gameObject;
        A42 = transform.GetChild(1).gameObject;
        A43 = transform.GetChild(2).gameObject;
        A44 = transform.GetChild(3).gameObject;
        A45 = transform.GetChild(4).gameObject;

        A41.SetActive(false);
        A42.SetActive(false);
        A43.SetActive(false);
        A44.SetActive(false);
        A45.SetActive(false);

    }
    void Start()
    {
        StartCoroutine(A4Coroutine());
    }
    IEnumerator A4Coroutine()                                   //약 10초간 Active 상태여야 함.     
    {
        yield return new WaitForSeconds(0.5f);
        A41.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        A42.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        A43.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        A44.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        A45.SetActive(true);
        yield return new WaitForSeconds(7.5f);
        A41.SetActive(false);
        A42.SetActive(false);
        A43.SetActive(false);
        A44.SetActive(false);
        A45.SetActive(false);

    }
}
