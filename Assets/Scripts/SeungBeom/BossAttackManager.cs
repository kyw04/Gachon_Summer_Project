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
        for (int i = 0; i < P2MaxCount; i++)
        {
            P2_PoolObject = Instantiate(P2_Atk);
            P2queue.Enqueue(P2_PoolObject);
            P2_PoolObject.SetActive(false);
            //P2_PoolObject.hideFlags = HideFlags.HideInHierarchy;
        }
        // ----------------------------------------- 3번패턴 오브젝트 풀 -----------------------------------
        for (int i = 0; i < P3MaxCount; i++)
        {
            P3_PoolObject = Instantiate(P3_Atk);
            P3queue.Enqueue(P3_PoolObject);
            P3_PoolObject.SetActive(false);
            //P3_PoolObject.hideFlags = HideFlags.HideInHierarchy;
        }
        StartCoroutine(Pattern1());
        StartCoroutine(Pattern2());
    }
    //---------------------------------------- P1 오브젝트 풀 관리 함수 -----------------------------
    public GameObject P1_GetItem()
    {
        P1_PoolObject = P1queue.Dequeue();
        P1_PoolObject.SetActive(true);
        P1queue.Enqueue(P1_PoolObject);
        return P1_PoolObject;
    }
    


    //---------------------------------------- P1 오브젝트 풀 관리 함수 -----------------------------

    //---------------------------------------- P2 오브젝트 풀 관리 함수 -----------------------------
    public GameObject P2_GetItem()
    {
        P2_PoolObject = P2queue.Dequeue();
        P2_PoolObject.SetActive(true);
        P2queue.Enqueue(P2_PoolObject);
        return P2_PoolObject;
    }
    //---------------------------------------- P2 오브젝트 풀 관리 함수 -----------------------------

    //---------------------------------------- P3 오브젝트 풀 관리 함수 -----------------------------
    public GameObject P3_GetItem()
    {
        P3_PoolObject = P3queue.Dequeue();
        P3_PoolObject.SetActive(true);
        P3queue.Enqueue(P3_PoolObject);
        return P3_PoolObject;
    }
    //---------------------------------------- P3 오브젝트 풀 관리 함수 -----------------------------


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
    IEnumerator Pattern2()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.5f);

            if (P2)
            {
                P2_GetItem();
            }
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
            P2 = true;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            P3 = false;
        }
    }



}
