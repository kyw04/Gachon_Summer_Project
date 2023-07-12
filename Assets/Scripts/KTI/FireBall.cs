using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Bullet
{
    Rigidbody rigid;
    float angularPower = 2;
    float scaleValue = 0.1f;
    bool isShoot;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();  
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }

    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(5.2f);
        isShoot = true;
    }

    IEnumerator GainPower()
    {
        while (!isShoot)
        {
            angularPower += 5.55f;
            scaleValue += 0.005f;
            transform.localScale = Vector3.one * scaleValue;
            rigid.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
            yield return null;
        }

    }
   
}

