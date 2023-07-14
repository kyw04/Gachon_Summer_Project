using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk4Ex : MonoBehaviour
{
    public GameObject Warning;
    public GameObject Laser;
    public GameObject Explosion;
    bool canattacked;
    GameObject Target;

    SphereCollider sphere;
    // Start is called before the first frame update
    void OnEnable()
    {
        Target = FindObjectOfType<PlayerComponent>().gameObject;

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
    IEnumerator Atk4()                              //ÃÑ 6.6ÃÊ.
    {
        yield return new WaitForSeconds(3f);

        Laser.SetActive(true);

        yield return new WaitForSeconds(0.4f);      //Æø¹ß ¹ß»ý

        Warning.SetActive(false);
        Explosion.SetActive(true);
        sphere.enabled = true;
        yield return new WaitForSeconds(0.01f);
        if (canattacked)
        {
            Target.SendMessage("Damaged", 5f);
        }
        yield return new WaitForSeconds(0.19f);
        sphere.enabled = false;

        yield return new WaitForSeconds(3);

        gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == Target)
        {
            canattacked = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Target)
        {
            canattacked = false;
        }
    }
}
