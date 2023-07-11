using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackManager : MonoBehaviour
{
    //----------------------------------- 1번 패턴 오브젝트 풀 ------------------------------------
    GameObject P1Bullet;
    private Queue<GameObject> P1queue = new Queue<GameObject>();            //1번 패턴 오브젝트 풀
    [Header("[패턴 1] 오브젝트 수 / 총알 생성 간격")]
    public int P1MaxCount = 125;
    public float FireRate = 0.3f;
    //---------------------------------------------------------------------------------------------

    //----------------------------------- 2번 패턴 오브젝트 풀 ------------------------------------
    GameObject P2_Atk;
    private Queue<GameObject> P2queue = new Queue<GameObject>();            //2번 패턴 오브젝트 풀
    [Header("[패턴 2] 오브젝트 수")]
    public int P2MaxCount = 10;
    //---------------------------------------------------------------------------------------------

    //----------------------------------- 3번 패턴 오브젝트 풀-------------------------------------
    GameObject P3_Atk;
    private Queue<GameObject> P3queue = new Queue<GameObject>();            //3번 패턴 오브젝트 풀
    [Header("[패턴 3] 오브젝트 수")]
    public int P3MaxCount = 3;
    //---------------------------------------------------------------------------------------------


    //---------------- 2번패턴 리스트로 관리해보자. ---------------------------
    private List<GameObject> P2list = new List<GameObject>();
    //---------------- 2번패턴 리스트로 관리해보자. ---------------------------




    public int ListSentinel = 0;





    [Header("장판기 (메테오, 폭발)이 생성되는 위치의 정확도 조정")]
    public float MagicAccuracy;
    public Vector3 offset;

    public GameObject player;

    bool P1;
    bool P2;
    bool P3;

    GameObject P1_PoolObject;
    GameObject P2_PoolObject;
    GameObject P3_PoolObject;



    private void Awake()
    {
        P1 = false;
        P2 = false;
        P3 = false;

        P1Bullet = Resources.Load<GameObject>("SeungBeom/FinalBullet 1");
        P2_Atk = Resources.Load<GameObject>("SeungBeom/Atk2Ex 1");
        P3_Atk = Resources.Load<GameObject>("SeungBeom/Atk3Ex 1");


    }

    void Start()
    {
        StartCoroutine(p2list());
        player = GameObject.FindGameObjectWithTag("Player");
        // ----------------------------------------- 1번패턴 오브젝트 풀 -----------------------------------
        for (int i = 0; i < P1MaxCount; i++)
        {
            P1_PoolObject = Instantiate(P1Bullet);
            P1queue.Enqueue(P1_PoolObject);
            P1_PoolObject.SetActive(false);
            P1_PoolObject.hideFlags = HideFlags.HideInHierarchy;
        }
        // ----------------------------------------- 2번패턴 오브젝트 풀 -----------------------------------
        /*
        for (int i = 0; i < P2MaxCount; i++)
        {
            P2_PoolObject = Instantiate(P2_Atk);
            P2queue.Enqueue(P2_PoolObject);
            P2_PoolObject.SetActive(false);
            //P2_PoolObject.hideFlags = HideFlags.HideInHierarchy;
        
        }
        */
        // ----------------------------------------- 3번패턴 오브젝트 풀 -----------------------------------
        for (int i = 0; i < P3MaxCount; i++)
        {
            P3_PoolObject = Instantiate(P3_Atk);
            P3queue.Enqueue(P3_PoolObject);
            P3_PoolObject.SetActive(false);
            P3_PoolObject.hideFlags = HideFlags.HideInHierarchy;
        }

        StartCoroutine(Pattern1());

        for(int i =0; i < 3; i ++)      // 리스트로 관리하는 2번패턴. 3개까지 생성하고 재사용 할 것임.
        {
            P2_PoolObject = Instantiate(P2_Atk);
            P2list.Insert(i,P2_PoolObject);         //리스트 0,1,2 에 2번패턴 생성
            P2_PoolObject.SetActive(false);
        }
    }
    //---------------------------------------- P1,2,3 오브젝트 풀 관리 함수 -----------------------------
    public GameObject P1_GetItem()
    {
        P1_PoolObject = P1queue.Dequeue();
        P1_PoolObject.SetActive(true);
        P1queue.Enqueue(P1_PoolObject);
        return P1_PoolObject;
    }
    public GameObject P2_GetItem()
    {
        P2_PoolObject = P2queue.Dequeue();
        P2_PoolObject.SetActive(true);
        P2queue.Enqueue(P2_PoolObject);
        return P2_PoolObject;
    }
    public GameObject P3_GetItem()
    {
        P3_PoolObject = P3queue.Dequeue();
        P3_PoolObject.SetActive(true);
        P3queue.Enqueue(P3_PoolObject);
        return P3_PoolObject;
    }
    //------------------------------------------- 오브젝트 풀 관리 함수 --------------------------------


    IEnumerator Pattern1()
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
    IEnumerator p2list()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            P2list[ListSentinel].transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            yield return new WaitForSeconds(0.1f);
            P2list[ListSentinel].SetActive(true);   // 꺼내는 게 아니라 활성화만 시켜주는 것. 패턴의 작동이 끝난 후 2번패턴에 붙은 스크립트로 인해 알아서 enable 될 것임.
            Debug.Log("리스트 센티넬 값 : " + ListSentinel);
            yield return new WaitForSeconds(1);
            if (ListSentinel <= 2) ListSentinel += 1;
            if (ListSentinel >= 3) ListSentinel = 0;
            yield return new WaitForSeconds(2);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            P1 = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            P1 = false;
        }


        if (Input.GetKeyDown(KeyCode.N))
        {
            
        }

    }



}
