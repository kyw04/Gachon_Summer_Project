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

    public int Sentinel;

    Vector3 Reposition;
    Vector3 Reposition1;
    Vector3 Reposition2;

    void OnEnable()
    {
        
        Projectile = transform.GetChild(0).gameObject;              // ����ü
        HitPrefab = transform.GetChild(1).gameObject;               // �ǰ� ����Ʈ

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
            Accuracy = 1 - (Accuracy / 100);            // ���߷��� 0.~ Ȥ�� 1�� ǥ���ȴ�.

            for(int i = 0; i <3; i++)
            {
                float Luck = Random.Range(-Accuracy, Accuracy);
                float Spread = Random.Range(0, 2); // 0 Ȥ�� 1

                if(i == 0)                              // ù ��° , Y������ �����ϰ� ź�� Ƣ���� ��.
                {
                    if(Spread == 0) Reposition1 = new Vector3(0, Luck, 0);
                    else Reposition1 = new Vector3(0, -Luck, 0);

                }
                if (i == 1)                                     //�� ��° �ݺ� , Z ������ �����ϰ� ź�� Ƣ���� ��.
                {
                    if(Spread == 0) Reposition2 = new Vector3(0, 0, Luck);
                    else Reposition2 = new Vector3(0, 0, -Luck); 
                }
                if (i == 2) Reposition = new Vector3(0, Reposition1.y, Reposition2.z);  //������ �ݺ� , �ռ� �ݺ����� ��ȭ�� �� ���� position�� ��� ����.
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
        //HitPrefab.transform.position = CP.point;              //������������ �ǰ� ����Ʈ ��ġ ����
        StartCoroutine(BulletHit());
       if(other.gameObject == Target)
        {
            Target.SendMessage("Damaged", 0.5f);
        }
    }

    IEnumerator BulletHit()         //��ź �� �Ѿ��� ������ �����մϴ�.
    {
        Rb.velocity = Vector3.zero;                 // �Ѿ��� �ӵ��� 0�� �˴ϴ�.'
        Rb.angularVelocity = Vector3.zero;

        Sentinel = 1;

        Accuracy = 100;                             //�Ѿ� ��Ȱ�� �� ź������ �� *�߰�* ���� �ʵ���.

        Rb.constraints = RigidbodyConstraints.FreezeAll;

        BoxCol.enabled = false;                     //collider �� ������� �ʰ� �մϴ�.

        Projectile.SetActive(false);                // ����ü(�Ѿ�)�� ��Ȱ��ȭ �մϴ�.

        HitPrefab.SetActive(true);                  // ��ź ����Ʈ�� Ȱ��ȭ�մϴ�


        yield return new WaitForSeconds(1.2f);
        Rb.constraints = RigidbodyConstraints.None;

        gameObject.SetActive(false);                //��ź �������κ��� 1.2�� ��, �Ѿ��� ��Ȱ��ȭ �մϴ�.

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
