using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiBullet : MonoBehaviour
{
    public GameObject Projectile;       //총알. 충돌시 사라질 것.
    public GameObject HitPrefab;        //특정 위치에 파티클 생성 후, 그곳에서 총알이 발사되도록 할 것임. 맞은위치에만 파티클 생성할 것.

    public float Accuracy; //명중률. 
    public float Speed;

    Rigidbody Rb;
    BoxCollider BoxCol;
    GameObject Target;

    public int Sentinel;

    Vector3 Reposition;
    Vector3 Reposition1;
    Vector3 Reposition2;

    void OnEnable()
    {
        
        Projectile = transform.GetChild(0).gameObject;              // 투사체
        HitPrefab = transform.GetChild(1).gameObject;               // 피격 이펙트

        HitPrefab.SetActive(false);


        if(Sentinel == 1)
        {
            Refresh();
        }



        Target = FindObjectOfType<PlayerComponent>().gameObject;
        Rb = GetComponent<Rigidbody>();
        BoxCol = GetComponent<BoxCollider>();

        

        transform.LookAt(Target.transform);

        if(Accuracy != 100)
        {
            Accuracy = 1 - (Accuracy / 100);            // 명중률은 0.~ 혹은 1로 표현된다.

            for(int i = 0; i <3; i++)
            {
                float Luck = Random.Range(-Accuracy, Accuracy);
                float Spread = Random.Range(0, 2); // 0 혹은 1

                if(i == 0)                              // 첫 번째 , Y축으로 랜덤하게 탄이 튀도록 함.
                {
                    if(Spread == 0) Reposition1 = new Vector3(0, Luck, 0);
                    else Reposition1 = new Vector3(0, -Luck, 0);

                }
                if (i == 1)                                     //두 번째 반복 , Z 축으로 랜덤하게 탄이 튀도록 함.
                {
                    if(Spread == 0) Reposition2 = new Vector3(0, 0, Luck);
                    else Reposition2 = new Vector3(0, 0, -Luck); 
                }
                if (i == 2) Reposition = new Vector3(0, Reposition1.y, Reposition2.z);  //마지막 반복 , 앞선 반복에서 변화된 각 축의 position을 모두 받음.
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Rb.velocity += (transform.forward + Reposition) * (Speed * Time.deltaTime);

        Projectile.transform.Rotate(new Vector3(0, 0, 5* 720 * Time.deltaTime));
    }
    private void OnTriggerEnter(Collider other)
    {
        //ContactPoint CP = collision.GetContact(0);
        //HitPrefab.transform.position = CP.point;              //접촉지점으로 피격 이펙트 위치 고정
        StartCoroutine(BulletHit());
       if(other.gameObject == Target)
        {
            Target.SendMessage("Damaged", 0.5f);
        }
    }

    IEnumerator BulletHit()         //착탄 후 총알의 동작을 관리합니다.
    {
        Rb.velocity = Vector3.zero;                 // 총알의 속도는 0이 됩니다.'
        Rb.angularVelocity = Vector3.zero;

        Sentinel = 1;

        Accuracy = 100;                             //총알 재활용 시 탄퍼짐이 더 *추가* 되지 않도록.

        Rb.constraints = RigidbodyConstraints.FreezeAll;

        BoxCol.enabled = false;                     //collider 를 사용하지 않게 합니다.

        Projectile.SetActive(false);                // 투사체(총알)을 비활성화 합니다.

        HitPrefab.SetActive(true);                  // 착탄 이펙트를 활성화합니다


        yield return new WaitForSeconds(1.2f);
        Rb.constraints = RigidbodyConstraints.None;

        gameObject.SetActive(false);                //착탄 시점으로부터 1.2초 후, 총알을 비활성화 합니다.

    }

    private void Refresh()
    {

        Projectile.SetActive(true);
        transform.position = new Vector3(0, 30, 0);
        BoxCol.enabled = true;
    }





    /*
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint CP = collision.GetContact(0);

        HitPrefab.transform.position = CP.point;              //접촉지점으로 피격 이펙트 위치 고정

        Rb.velocity = Vector3.zero;

        BoxCol.enabled = false;



        Projectile.SetActive(false);
        Debug.Log("총알 없어짐 / 이펙트 출력");
        Rb.velocity = Vector3.zero;

        HitPrefab.SetActive(true);
    }
    */


}
