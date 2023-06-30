using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk1Bullet : MonoBehaviour
{
    GameObject Target;
    Rigidbody rigid;
    public float Pattern1BulletSpeed;
    void Start()
    {
        StartCoroutine(BulletDestroy());

        rigid = GetComponent<Rigidbody>();
        Target = GameObject.FindGameObjectWithTag("Player");
        Vector3 Dir = (Target.transform.position - transform.position).normalized;
        rigid.AddForce(Dir * Pattern1BulletSpeed);


    }
    private void OnCollisionEnter(Collision collision)      //총알이 어디든 부딛히면 파괴됨.
    {
        Destroy(gameObject);
    }
    IEnumerator BulletDestroy()
    {
        yield return new WaitForSeconds(3);     //생성된지 3초 후에 총알이 파괴되도록.
        Destroy(gameObject);
    }
}
