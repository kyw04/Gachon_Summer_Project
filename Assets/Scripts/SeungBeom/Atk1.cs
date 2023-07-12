using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk1 : MonoBehaviour
{
    public GameObject Bullet;
    public Atk1Pool atk1pool;

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
            atk1pool.GetItem();
        }
    }
}
