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
    [SerializeField] int Boss_AttackNum;

    [SerializeField] int ranNum;

    public ObjectPoolComponent boss_Attack;
    float attackDelayTime;

    public GameObject die_Effect;

    float away;
    public Transform Player;

    // Transform playerTransform;
    IEnumerator attack;

    Vector3 dir;

    public float dirX;
    public float dirZ;

    public enum EnemyState
    {
        Idle,
        Walk,
        Attack1,
        Attack2,
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

        //GameObject player = GameObject.Find("Player");
        //if (player != null)
        //{
        //    playerTransform = player.transform;
        //}
        attack = Attack_C();
        StartCoroutine(attack);
        //  StartCoroutine(Move_C());



    }

    void Update()
    {
        away = Vector3.Distance(transform.position, Player.position);
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
                Walk(dir);
                break;

            case EnemyState.Attack1:
                Attack();
                break;

            case EnemyState.Dead:
                Dead();
                break;
        }
    }
    public void Idle()
    {
        // 플레이어와 약간 가까워지면
        if (away <= 10)
        {
            // 이동 상태로 전환
            eState = EnemyState.Walk;
            // 이동 애니메이션으로 전환
            anim.SetFloat("Z", 1);
        }
    }

    public void Walk(Vector3 dir)
    {
        //dir = Vector3.Lerp(dir, new Vector3(dirX, 0, dirZ), Time.deltaTime * 5);
        //anim.SetFloat("Z", dir.z);
        //anim.SetFloat("X", dir.x);

        if (away <= 5)
        {
            // 공격 상태가 되자마자 때리도록
            attackDelayTime = 1;
            eState = EnemyState.Attack1;
        }
        // 거리가 멀어졌을 때 기본 상태
        else if (away > 10)
        {
            eState = EnemyState.Idle;

            // 기본 애니메이션으로 전환
            anim.SetFloat("Z", 0);

            // 길찾기 중지
            agent.isStopped = true;
        }
        //플레이어를 향해 이동
        else
        {
            Debug.Log("플레이어에게 가는중");
            anim.SetFloat("Z", 1);
            // 계속해서 플레이어를 향해 이동하도록 목적지 설정 (destination)
            agent.destination = Player.position;
            // 길찾기 시작
            agent.isStopped = false;
        }
    }

    void Dead()
    {
        die_Effect.SetActive(true);
        anim.SetTrigger("dead"); 
        Destroy(gameObject, 5f);
        Destroy(die_Effect, 5f);
    }
    void Attack()
    {
        // 진행 시간 누적
        attackDelayTime += Time.deltaTime;

        // 1초가 지나면
        if (attackDelayTime >= 1)
        {
            // 공격 애니메이션으로 전환
            anim.SetTrigger("attack");

            // 0으로 초기화해서 다시 처음부터 1초 세기
            attackDelayTime = 0;
        }
        // 플레이어가 멀어지면
        if (away > 5)
        {
            // 이동 상태로 전환
            eState = EnemyState.Walk;
        }
    }

    IEnumerator Move_C()
    {
        while (true)
        {
            dirX = Random.Range(-1, 2);
            dirZ = Random.Range(-1, 2);

            rb.velocity = new Vector3(dirX, 0, dirZ);
            yield return new WaitForSeconds(2f);

        }
    }

    IEnumerator Attack_C()
    {
        while (true)
        {
            switch (Boss_AttackNum)
            {
                // 낫 한번 휘두르기 
                case 1:

                    break;
                // 낫 두번 휘두르기 
                case 2:

                    break;
                // 마법 공격(메테오 소환)
                case 3:
                    // 플레이어의 위치를 얻어옴
                    Vector3 playerPosition = Player.position;
                    // 메테오를 플레이어의 현재 위치에 생성
                    Vector3 meteorPosition = new Vector3(playerPosition.x, playerPosition.y + 3f, playerPosition.z);
                    GameObject meteor = boss_Attack.GetItem(meteorPosition);
                    meteor.GetComponent<Meteor_2Stage>().boss_Attack = this.boss_Attack;
                    //// GameObject meteor = Instantiate(meteorPrefab, meteorPosition, Quaternion.identity);
                    if (meteor.transform.position.y <= -0.5)
                        boss_Attack.FreeItem(meteor);


                    // 생성된 메테오에 대한 추가 설정이 필요하다면 여기에 추가 코드 작성
                    break;
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
