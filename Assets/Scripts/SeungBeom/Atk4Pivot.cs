using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk4Pivot : MonoBehaviour
{
    Transform TARGET;
    //Transform Targetposition;
    GameObject one;
    GameObject two;
    GameObject three;
    GameObject four;
    GameObject five;
    GameObject six;
    GameObject seven;
    GameObject eight;

    GameObject Laser;
    GameObject EndPos;
    bool Lase;
    private void Awake()

    {
        TARGET = GameObject.FindGameObjectWithTag("Player").transform;

        transform.LookAt(new Vector3(TARGET.position.x, transform.position.y, TARGET.position.z));

        one = transform.GetChild(0).gameObject;
        two = transform.GetChild(1).gameObject;
        three = transform.GetChild(2).gameObject;
        four = transform.GetChild(3).gameObject;
        five = transform.GetChild(4).gameObject;
        six = transform.GetChild(5).gameObject;
        seven = transform.GetChild(6).gameObject;
        eight = transform.GetChild(7).gameObject;

        Laser = transform.GetChild(8).gameObject;
        EndPos = transform.GetChild(9).gameObject;

        
        Laser.SetActive(false);
    }
    private void OnEnable()
    {
        StartCoroutine(Atk4pivot());
        one.SetActive(false);
        two.SetActive(false);
        three.SetActive(false);
        four.SetActive(false);
        five.SetActive(false);
        six.SetActive(false);
        seven.SetActive(false);
        eight.SetActive(false);
        Laser.transform.LookAt(this.transform.position);
    }




    private void Update()
    {
        if (Lase)
        {
            Laser.transform.rotation = Quaternion.Slerp(Laser.transform.rotation, Quaternion.LookRotation(EndPos.transform.position - Laser.transform.position), Time.deltaTime * 3.2f);

        }
        if (!Lase)
        {
            Laser.transform.LookAt(this.transform.position);
        }
    }
    IEnumerator Atk4pivot()
    {
        Laser.SetActive(true);
        transform.LookAt(new Vector3(TARGET.position.x, transform.position.y, TARGET.position.z));
        Lase = true;

        yield return new WaitForSeconds(0.35f);
        eight.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        one.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        two.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        three.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        four.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        five.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        six.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        seven.SetActive(true);
        Lase = false;
        Laser.SetActive(false);
    }
}