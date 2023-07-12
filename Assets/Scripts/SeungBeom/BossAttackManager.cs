using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackManager : MonoBehaviour
{

    [Header("보스 공격 주기")]
    public float WaitTime;
    //----------------------------------- 1번 패턴 오브젝트 풀 ------------------------------------
    GameObject P1Bullet;
    private Queue<GameObject> P1queue = new Queue<GameObject>();            
    [Header("[패턴 1] 오브젝트 수 / 총알 생성 간격")]
    public int P1MaxCount = 125;
    public float FireRate = 0.3f;
    //----------------------------------- 2번 패턴 오브젝트 풀 ------------------------------------
    GameObject P2_Atk;
    private List<GameObject> P2list = new List<GameObject>();
    [Header("[패턴 2] 오브젝트 수")]
    public int P2MaxCount = 10;
    public int ListSentinel = 0;

    public float Accuracy;
    Vector3 Reposition;




    




    GameObject P3_Atk;
    GameObject P4_Atk;
    GameObject P5_Atk;
    GameObject Shield;
    int Selection;


    public GameObject player;

    bool P1;
    bool P2;
    bool P3;

    GameObject P1_PoolObject;
    GameObject P2_ListObject;
    GameObject P3_ListObject;



    private void Awake()
    {
        P1 = false;


        P1Bullet = Resources.Load<GameObject>("SeungBeom/FinalBullet 1");
        P2_Atk = Resources.Load<GameObject>("SeungBeom/Atk2Ex 1");
        P3_Atk = transform.GetChild(6).gameObject;
        P4_Atk = transform.GetChild(4).gameObject;
        Shield = transform.GetChild(5).gameObject;

    }

    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        // ----------------------------------------- 1번패턴 오브젝트 풀 -----------------------------------
        for (int i = 0; i < P1MaxCount; i++)
        {
            P1_PoolObject = Instantiate(P1Bullet);
            P1queue.Enqueue(P1_PoolObject);
            P1_PoolObject.SetActive(false);
            P1_PoolObject.hideFlags = HideFlags.HideInHierarchy;
        }
        for(int i =0; i < 3; i ++)      // 리스트로 관리하는 2번패턴.
        {
            P2_ListObject = Instantiate(P2_Atk);
            P2list.Insert(i, P2_ListObject);         //리스트 0,1,2 에 2번패턴 생성
            P2_ListObject.SetActive(false);
        }

        P3_Atk.SetActive(false);
        P4_Atk.SetActive(false);
        Shield.SetActive(false);
        //항상 실행해야 할 것
        StartCoroutine(FIrstPattern());
        StartCoroutine(BulletShoot());
        StartCoroutine(Select());
        StartCoroutine(Barrier());
    }

    public GameObject P1_GetItem()
    {
        P1_PoolObject = P1queue.Dequeue();
        P1_PoolObject.SetActive(true);
        P1queue.Enqueue(P1_PoolObject);
        return P1_PoolObject;
    }
    IEnumerator BulletShoot()
    {
        while(true)
        {
            yield return new WaitForSeconds(FireRate);
            if (P1)
            {
                P1_GetItem();
            }
        }
    }
    IEnumerator FIrstPattern()  //완료
    {
        P1 = true;
        yield return new WaitForSeconds(7);  //몇초동안 발사할것인지? 7초로 하자.
        P1 = false;
        yield return new WaitForSeconds(WaitTime);
        NextPattern();
    }
    IEnumerator SecondPattern() //완료
    {
        Accura();
        P2list[0].transform.position = new Vector3(player.transform.position.x + Reposition.x, 0, player.transform.position.z + Reposition.z);
        yield return new WaitForSeconds(0.1f);
        P2list[0].SetActive(true);
        yield return new WaitForSeconds(4f); //1번
        Accura();
        P2list[1].transform.position = new Vector3(player.transform.position.x + Reposition.x, 0, player.transform.position.z + Reposition.z);
        yield return new WaitForSeconds(0.1f);
        P2list[1].SetActive(true);
        yield return new WaitForSeconds(4f); //2번
        Accura();
        P2list[2].transform.position = new Vector3(player.transform.position.x + Reposition.x, 0, player.transform.position.z + Reposition.z);
        yield return new WaitForSeconds(0.1f);
        P2list[2].SetActive(true);
        yield return new WaitForSeconds(WaitTime); //3번
        NextPattern();
        /*
        if (P2)
        {
            P2list[ListSentinel].transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            yield return new WaitForSeconds(0.1f);
            P2list[ListSentinel].SetActive(true);   // 꺼내는 게 아니라 활성화만 시켜주는 것. 패턴의 작동이 끝난 후 2번패턴에 붙은 스크립트로 인해 알아서 enable 될 것임.
            Debug.Log("리스트 센티넬 값 : " + ListSentinel);
            yield return new WaitForSeconds(1);
            if (ListSentinel <= 2) ListSentinel += 1;
            if (ListSentinel >= 3) ListSentinel = 0;
            yield return new WaitForSeconds(2);
        }
        */
    }
    IEnumerator ThirdPattern()  //완료
    {
        P3_Atk.transform.position = player.transform.position;
        yield return new WaitForSeconds(0.1f);
        P3_Atk.SetActive(true);
        yield return new WaitForSeconds(10f);
        yield return new WaitForSeconds(WaitTime - (WaitTime/2));
        NextPattern();
    }
    IEnumerator FourthPattern()
    {
        P4_Atk.SetActive(true);
        yield return new WaitForSeconds(10);
        P4_Atk.SetActive(false);
        yield return new WaitForSeconds(WaitTime);
        NextPattern();
    }
    IEnumerator Barrier()
    {
        while(true)
        {
            Shield.SetActive(true);
            yield return new WaitForSeconds(6f);
            Shield.SetActive(false);
            yield return new WaitForSeconds(45f);
        }
    }

    void NextPattern()
    {
        switch(Selection)
        {
            case 1:
                StartCoroutine(FIrstPattern());
                break;
            case 2:
                StartCoroutine(SecondPattern());
                break;
            case 3:
                StartCoroutine(ThirdPattern());
                break;
            case 4:
                StartCoroutine(FourthPattern());
                break;
        }
    }
    void Accura()
    {
        if (Accuracy != 100)
        {
            Accuracy = 1 - (Accuracy / 100);            

            for (int i = 0; i < 2; i++)
            {
                float Luck = Random.Range(-Accuracy, Accuracy);
                float Spread = Random.Range(0, 2);

                if (i == 0)                              
                {
                    if (Spread == 0) Reposition = new Vector3(0, Luck * 35, 0);
                    else Reposition = new Vector3(0, -Luck * 35, 0);
                }
                else                                    
                {
                    if (Spread == 0) Reposition = new Vector3(0, Reposition.y, Luck * 35);
                    else Reposition = new Vector3(0, Reposition.y, -Luck * 35);
                }
            }
        }
    }

    IEnumerator Select()
    {
        while(true)
        {
            yield return new WaitForSeconds(8f);
            Selection = 2;
            //Selection = Random.Range(1, 5); // 패턴이 5개이기 때문, (1~5 까지)
            Debug.Log(Selection);
        }
    }


    private void Update()
    {
    }



}
