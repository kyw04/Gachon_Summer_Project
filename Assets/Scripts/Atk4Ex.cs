using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk4Ex : MonoBehaviour
{
    public GameObject ReadyEffect;
    public GameObject Atk4Bullet;
    // Start is called before the first frame update
    void Start()
    {
        ReadyEffect = transform.GetChild(0).gameObject;
        Atk4Bullet = transform.GetChild(1).gameObject;

        ReadyEffect.SetActive(true);
        Atk4Bullet.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Atk4()
    {

        
        yield return new WaitForSeconds(2);
    }
}
