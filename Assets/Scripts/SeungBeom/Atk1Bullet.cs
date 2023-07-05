using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk1Bullet : MonoBehaviour
{
    GameObject Target;
    Rigidbody rigid;
    public float Pattern1BulletSpeed;
    void OnEnable()
    {
        rigid = GetComponent<Rigidbody>();
        Target = GameObject.FindGameObjectWithTag("Player");
        Vector3 Dir = (Target.transform.position - transform.position).normalized;
        rigid.AddForce(Dir * Pattern1BulletSpeed);
    }
    private void OnCollisionEnter(Collision collision)      //총알이 어디든 부딛히면 파괴됨.
    {
        gameObject.SetActive(false);
    }
    
}
