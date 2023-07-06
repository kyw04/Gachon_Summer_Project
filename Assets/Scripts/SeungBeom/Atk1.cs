using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk1 : MonoBehaviour
{
    public GameObject Bullet;
    public float Firerate;
    GameObject Target;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(BulletFire());
    }

    IEnumerator BulletFire()
    {
        while(true)
        {
            yield return new WaitForSeconds(Firerate);
            Instantiate(Bullet, transform.position, Quaternion.identity);
        }
    }
}
