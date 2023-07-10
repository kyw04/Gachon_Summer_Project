using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiBullet : MonoBehaviour
{
    public GameObject Projectile;       //�Ѿ�. �浹�� ����� ��.
    public GameObject HitPrefab;        //Ư�� ��ġ�� ��ƼŬ ���� ��, �װ����� �Ѿ��� �߻�ǵ��� �� ����. ������ġ���� ��ƼŬ ������ ��.

    public float Accuracy; //���߷�. 
    public float Speed;

    Rigidbody Rb;
    BoxCollider BoxCol;
    GameObject Target;


    Vector3 Reposition;

    void OnEnable()
    {

        Projectile = transform.GetChild(0).gameObject;              // ����ü
        HitPrefab = transform.GetChild(1).gameObject;               // �ǰ� ����Ʈ

        HitPrefab.SetActive(false);

        Target = GameObject.FindGameObjectWithTag("Player");
        Rb = GetComponent<Rigidbody>();
        BoxCol = GetComponent<BoxCollider>();


        transform.LookAt(Target.transform);

        if(Accuracy != 100)
        {
            Accuracy = 1 - (Accuracy / 100);            // ���߷��� 0.~ Ȥ�� 1�� ǥ���ȴ�.

            for(int i = 0; i <2; i++)
            {
                float Luck = Random.Range(-Accuracy, Accuracy);
                float Spread = Random.Range(0, 2); // 0 Ȥ�� 1

                if(i == 0)                              // ù ��° �ݺ� , Y������ �����ϰ� ź�� Ƣ���� ��.
                {
                    if(Spread == 0) Reposition = new Vector3(0, Luck, 0);
                    else Reposition = new Vector3(0, -Luck, 0);
                }
                else                                    //�� ��° �ݺ� , Z ������ �����ϰ� ź�� Ƣ���� ��.
                {
                    if(Spread == 0) Reposition = new Vector3(0, Reposition.y, Luck);
                    else Reposition = new Vector3(0, Reposition.y, -Luck);
                    
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Rb.velocity += (transform.forward + Reposition) * (Speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        //ContactPoint CP = collision.GetContact(0);
        //HitPrefab.transform.position = CP.point;              //������������ �ǰ� ����Ʈ ��ġ ����
        StartCoroutine(BulletHit());
       
    }

    IEnumerator BulletHit()         //��ź �� �Ѿ��� ������ �����մϴ�.
    {
        Rb.velocity = Vector3.zero;                 // �Ѿ��� �ӵ��� 0�� �˴ϴ�.

        BoxCol.enabled = false;                     //collider �� ������� �ʰ� �մϴ�.

        Projectile.SetActive(false);                // ����ü(�Ѿ�)�� ��Ȱ��ȭ �մϴ�.
        Debug.Log("�Ѿ� ������ / ����Ʈ ���");

        HitPrefab.SetActive(true);                  // ��ź ����Ʈ�� Ȱ��ȭ�մϴ�


        yield return new WaitForSeconds(1.2f);


        gameObject.SetActive(false);                //��ź �������κ��� 1.2�� ��, �Ѿ��� ��Ȱ��ȭ �մϴ�.

    }





    /*
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint CP = collision.GetContact(0);

        HitPrefab.transform.position = CP.point;              //������������ �ǰ� ����Ʈ ��ġ ����

        Rb.velocity = Vector3.zero;

        BoxCol.enabled = false;



        Projectile.SetActive(false);
        Debug.Log("�Ѿ� ������ / ����Ʈ ���");
        Rb.velocity = Vector3.zero;

        HitPrefab.SetActive(true);
    }
    */


}
