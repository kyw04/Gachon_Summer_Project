using JetBrains.Annotations;
using JYH;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Stage2_Boss : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;


    Rigidbody rb;

    [Header("원형으로 공격하는 프리팹 관련 변수들")]
    public float meteorSpeed;
    [SerializeField] Transform[] spawnPoint;


    public ObjectPoolComponent[] boss_Attack;
    public GameObject die_Effect;

    float away;
    public Transform Player;
    Vector3 dir;

    float delay_Time;

    public float dirZ;
    public enum EnemyState
    {
        Idle,
        Walk,
        Attack,
        Damaged,
        Dead
    }
    public EnemyState eState;


    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {

        Player = FindObjectOfType<PlayerCtrl>().transform;
        eState = EnemyState.Idle;
    }

    void Update()
    {
        away = Vector3.Distance(transform.position, Player.position); // 적과 플레이어의 거리
        Vector3 playerPosition = Player.position; // 플레이어의 위치

        //  Debug.Log(away);

        if (Input.GetKeyDown(KeyCode.Q))
            eState = EnemyState.Dead;

        ChangeEnemyState();
    }

    void ChangeEnemyState()
    {
        switch (eState)
        {
            case EnemyState.Idle:
                Idle();
                break;

            case EnemyState.Walk:
                Walk();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Dead:
                Dead();
                break;
        }
    }
    public void Idle()
    {
        agent.isStopped = true; // 길찾기 종료
        StartCoroutine(Idle_Delay());
        eState = EnemyState.Walk;
    }
    IEnumerator Idle_Delay()
    {
        anim.StopPlayback();
        yield return new WaitForSeconds(2f);
    }
    public void Walk()
    {
        agent.destination = Player.position;     // 계속해서 플레이어를 향해 이동하도록 목적지 설정 (destination)
        agent.isStopped = false; // 길찾기 시작
        // dir = Vector3.Lerp(dir, new Vector3(0, 0, dirZ), Time.deltaTime * 5);
        // anim.SetFloat("Z", dir.z);
        anim.SetFloat("Z", 1);
        if (away <= 10)
            eState = EnemyState.Attack;
    }
    void Dead()
    {
        die_Effect.SetActive(true);
        anim.SetTrigger("dead");
    }
    void Attack()
    {
        if (delay_Time <= Time.time)
        {
            delay_Time = Time.time + 3f;

            int attackNum = Random.Range(1, 4);
            Debug.Log(attackNum);
            switch (attackNum)
            {
                case 1:
                    Slash();
                    break;
                case 2:
                    CircleMeteorAttack();
                    break;
                case 3:
                    SpawnMeteor_Attack();
                    break;
            }
        }
    }

    public void Slash()
    {
        if (away <= 5)
            anim.SetTrigger("Slash");
    }
    public void CircleMeteorAttack() // 원형으로 메테오 생성
    {
        anim.SetTrigger("shootMeteor");
        Debug.Log("원형 메테오");
    }

    void Real_CircleMeteor_Attack()
    {
        for (int i = 0; i < spawnPoint.Length; i++)
        {
            Debug.Log("Real_CircleMeteor_Attack 들어옴");
            GameObject meteor0 = boss_Attack[1].GetItem(spawnPoint[i].position);
            Vector3 direction = spawnPoint[i].position - transform.position; // 현재 위치에서 spawnPoint 위치로 향하는 벡터
            meteor0.GetComponent<Rigidbody>().velocity = direction.normalized * meteorSpeed;
        }
    }

    void Off_Speed()
    {
        agent.speed = 0;
        //  eState = EnemyState.Idle;
    }

    void On_Speed()
    {
        agent.speed = 3.5f;
    }


    void SpawnMeteor_Attack() // 위에서 메테오 생성
    {
        anim.SetTrigger("spawn_Meteor");
        Debug.Log("sky메테오");
    }

    void Real_SpawnMeteor_Attack()
    {
        //    Debug.Log("Real_SpawnMeteor_Attack 들어옴");
        // 메테오를 플레이어의 현재 위치에 생성
        Vector3 meteorPosition = new Vector3(Player.position.x, Player.position.y + 3f, Player.position.z);
        GameObject meteor = boss_Attack[0].GetItem(meteorPosition);
        meteor.GetComponent<Meteor_2Stage>().boss_Attack = boss_Attack[0];
        if (meteor.transform.position.y <= -0.5)
            boss_Attack[0].FreeItem(meteor);

        eState = EnemyState.Idle;

    }

    void RealDead()
    {
        Destroy(gameObject);
    }
    void RealAttack()
    {
        if (away <= 3)
        {
            Player.SendMessage("Damaged", 0.2f);
        }
        eState = EnemyState.Idle;
    }
}
