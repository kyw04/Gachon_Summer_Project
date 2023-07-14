using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Bullet
{
    PlayerHPViewer playerHP;
    BossHPViewer bossHP;

    Rigidbody rigid;
    float angularPower = 2;
    float scaleValue = 0.1f;
    bool isShoot;

    private void Awake()
    {
        playerHP = GameObject.Find("Player").GetComponent<PlayerHPViewer>();
        bossHP = GameObject.Find("BossHPSlider").GetComponent<BossHPViewer>();
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            playerHP.Fireball();
        }

        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
            bossHP.Fire();
        }
    }
}

