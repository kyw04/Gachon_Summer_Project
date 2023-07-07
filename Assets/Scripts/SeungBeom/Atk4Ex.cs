using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk4Ex : MonoBehaviour
{
    public GameObject Warning;
    public GameObject Laser;
    public GameObject Explosion;


    SphereCollider sphere;
    // Start is called before the first frame update
    void OnEnable()
    {


        StartCoroutine(Atk4());
        Warning = transform.GetChild(0).gameObject;
        Laser = transform.GetChild(1).gameObject;
        Explosion = transform.GetChild(2).gameObject;
        sphere = GetComponent<SphereCollider>();

        Warning.SetActive(true);
        Laser.SetActive(false);
        Explosion.SetActive(false);
        sphere.enabled = false;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Atk4()                              //총 6.6초.
    {
        yield return new WaitForSeconds(3f);

        Laser.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        Warning.SetActive(false);
        Explosion.SetActive(true);
        sphere.enabled = true;

        yield return new WaitForSeconds(0.2f);

        sphere.enabled = false;

        yield return new WaitForSeconds(3);

        gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //플레이어에게 데미지 줄 것.
        }
    }
}
