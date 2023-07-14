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
    [Header("[패턴 2] 명중률 관리")]
    public float Accuracy;

    Vector3 P2Reposition;
    Vector3 P2Reposition1;
    Vector3 P2Reposition2;


    private List<GameObject> P5List = new List<GameObject>();
    int ListSentinel = 0;







    GameObject P3_Atk;
    GameObject P4_Atk;
    GameObject P5_Atk;
    GameObject Shield;

    GameObject P1Shield;

    int Selection;


    public GameObject player;

    bool P1;
    bool P2;
    bool P3;

    GameObject P1_PoolObject;
    GameObject P2_ListObject;
    GameObject P3_ListObject;

    GameObject P5_ListObject;

    Vector3 P5Reposition;
    Vector3 P5Reposition1;
    Vector3 P5Reposition2;

    private void Awake()
    {
        P1 = false;


        P1Bullet = Resources.Load<GameObject>("SeungBeom/FinalBullet 1");
        P2_Atk = Resources.Load<GameObject>("SeungBeom/Atk2Ex 1");
        P3_Atk = transform.GetChild(6).gameObject;
        P4_Atk = transform.GetChild(4).gameObject;
        P5_Atk = Resources.Load<GameObject>("SeungBeom/Atk5");
        Shield = transform.GetChild(5).gameObject;

        P1Shield = transform.GetChild(7).gameObject;
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
        for(int i = 0; i < 5; i++)
        {
            P5_ListObject = Instantiate(P5_Atk);
            P5List.Insert(i, P5_ListObject);
            P5_ListObject.SetActive(false);
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
                P1Shield.transform.rotation = Quaternion.Slerp(P1Shield.transform.rotation, Quaternion.LookRotation(player.transform.position - P1Shield.transform.position), Time.deltaTime * 8f);
            }
        }
    }
    IEnumerator FIrstPattern()  //완료
    {
        P1 = true;
        P1Shield.SetActive(true);
        yield return new WaitForSeconds(7);  //몇초동안 발사할것인지? 7초로 하자.
        P1 = false;
        yield return new WaitForSeconds(2);
        P1Shield.SetActive(false);
        yield return new WaitForSeconds(WaitTime - 2);
        NextPattern();
    }
    IEnumerator SecondPattern() //완료
    {
        P2Accuracy();
        P2list[0].transform.position = P2Reposition + new Vector3(player.transform.position.x, 0, player.transform.position.y);
        Debug.Log("메테오 생성 위치 : " + P2list[0].transform.position);
        yield return new WaitForSeconds(0.1f);
        P2list[0].SetActive(true);
        yield return new WaitForSeconds(4f); //1번
        P2Accuracy();
        P2list[1].transform.position = P2Reposition + new Vector3(player.transform.position.x, 0, player.transform.position.y);
        yield return new WaitForSeconds(0.1f);
        P2list[1].SetActive(true);
        yield return new WaitForSeconds(4f); //2번
        P2Accuracy();
        P2list[2].transform.position = P2Reposition + new Vector3(player.transform.position.x, 0, player.transform.position.y);
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
        P3_Atk.transform.position = new Vector3(player.transform.position.x,0,player.transform.position.z);
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
    IEnumerator FifthPattern()
    {
        P5Accuracy();
        P5List[0].transform.position = P5Reposition;
        yield return new WaitForSeconds(0.1f);
        P5List[0].SetActive(true);
        ListSentinel += 1;
        yield return new WaitForSeconds(1);
        P5Accuracy();
        P5List[1].transform.position = P5Reposition;
        yield return new WaitForSeconds(0.1f);
        P5List[1].SetActive(true);
        yield return new WaitForSeconds(1);
        P5Accuracy();
        P5List[2].transform.position = P5Reposition;
        yield return new WaitForSeconds(0.1f);
        P5List[2].SetActive(true);
        yield return new WaitForSeconds(1);
        P5Accuracy();
        P5List[3].transform.position = P5Reposition;
        yield return new WaitForSeconds(0.1f);
        P5List[3].SetActive(true);
        yield return new WaitForSeconds(1);
        P5Accuracy();
        P5List[4].transform.position = P5Reposition;
        yield return new WaitForSeconds(0.1f);
        P5List[4].SetActive(true);
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
            case 5:
                StartCoroutine(FifthPattern());
                break;
        }
    }
    void P2Accuracy()
    {
        if (Accuracy != 100)
        {
            Accuracy = 1 - (Accuracy / 100);            

            for (int i = 0; i < 3; i++)
            {
                float Luck = Random.Range(-Accuracy, Accuracy);
                float Spread = Random.Range(0, 2);

                if (i == 0)                              
                {
                    if (Spread == 0) P2Reposition1 = new Vector3(Luck * 5,0, 0);
                    else P2Reposition1 = new Vector3(-Luck * 5,0, 0);
                }
                if(i == 1)                                   
                {
                    if (Spread == 0) P2Reposition2 = new Vector3(0, 0, Luck * 5);
                    else P2Reposition2 = new Vector3(0, 0, -Luck * 5);
                }
                if(i == 2)
                {
                    P2Reposition = new Vector3(P2Reposition1.x, 0, P2Reposition2.z);
                }
            }
        }
    }
    void P5Accuracy()
    {
        for (int i = 0; i < 3; i++)
        {
            float Luck = Random.Range(-105, 106);
            float Spread = Random.Range(0, 2);
            
            if (i == 0)
            {
                
                if (Spread == 0) P5Reposition1 = new Vector3(Luck,0, 0);
                else P5Reposition1 = new Vector3(-Luck,0, 0);
            }
            if(i == 1)
            {
                if (Spread == 0) P5Reposition2 = new Vector3(0,0, Luck);
                else P5Reposition2 = new Vector3(0,0, -Luck);
            }
            if (i == 2)
            {
                P5Reposition = new Vector3(P5Reposition1.x, 0, P5Reposition2.z);
            }
        }
    }
    IEnumerator Select()
    {
        while(true)
        {
            yield return new WaitForSeconds(8f);
            Selection = Random.Range(1, 6); // 패턴이 5개이기 때문, (1~5 까지)
        }
    }
    


    private void Update()
    {
    }



}
