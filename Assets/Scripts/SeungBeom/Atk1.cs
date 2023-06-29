using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk1 : MonoBehaviour
{
    public GameObject Bullet;
    GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BulletFire());
    }

    // Update is called once per frame
    void Update()
    {
        
        Target = GameObject.FindGameObjectWithTag("Player");
        Vector3 Dir = Target.transform.position - transform.position;

    }

    IEnumerator BulletFire()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.5f);
            Instantiate(Bullet, transform.position, Quaternion.identity);
        }
    }
}
