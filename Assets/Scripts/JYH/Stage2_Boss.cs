using JetBrains.Annotations;
using JYH;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Stage2_Boss : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;

    [Header("보스체력 관련 UI")]
    public Slider hp_Bar;
    public int hp;
    [SerializeField] Text hp_T;
    Image fillImage;
    [SerializeField] Color32[] hp_Bar_Change_Color;


    Rigidbody rb;

    [Header("원형으로 공격하는 프리팹 관련 변수들")]
    public float meteorSpeed;
    [SerializeField] Transform[] spawnPoint;


    public ObjectPoolComponent[] boss_Attack;
    public ObjectPoolComponent[] boss_Partical;
    public GameObject die_Effect;


    [Header("보스 사운드")]
    [SerializeField] private AudioClip[] _clip;


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
        fillImage = hp_Bar.fillRect.GetComponentInChildren<Image>();
        Player = FindObjectOfType<PlayerComponent>().transform;
        eState = EnemyState.Idle;
        StartCoroutine(Destroy_Partical());


    }

    void Update()
    {
        away = Vector3.Distance(transform.position, Player.position); // 적과 플레이어의 거리
        Vector3 playerPosition = Player.position; // 플레이어의 위치

        //  Debug.Log(away);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Damaged(0.2f);
        }

        ChangeEnemyState();
    }
    void Damaged(float damage) // 플레이어가 공격했을때 이 함수 호출 
    {
        hp_Bar.value -= damage;
        if (hp_Bar.value <= 0f)
        {
            hp -= 1;
            hp_T.text = "X" + hp.ToString();
            hp_Bar.value = 1f;
            // 9-1/3 = 3 
            fillImage.color = hp_Bar_Change_Color[(hp - 1) / hp_Bar_Change_Color.Length];
            if (hp <= 0)
            {
                hp = 0;
                Boss_Dead();
                hp_Bar.value = 0f;
                hp_T.text = "X" + hp.ToString();
            }
        }
    }

    /*void RealAttack() // 플레이어가 공격했을때: 플레이어 스크립트에 넣을 예정인 코드
    {
        if (away<=2.5)
        {
             Enemy.SendMessage("Damaged", 입힐 데미지);
        }
    } 
    */
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
        {
            anim.SetTrigger("slash");
        }
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
            direction.y = 0f;
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
        GameObject partical1 = boss_Partical[0].GetItem(meteorPosition);

        boss_Attack[0].FreeItem(meteor, 2f);
        boss_Partical[0].FreeItem(partical1, 2f);


        eState = EnemyState.Idle;

    }

    IEnumerator Destroy_Partical()
    {
        yield return new WaitForSeconds(3f);
        Vector3 PlayerPosition = new Vector3(Player.position.x, Player.position.y - 5f, Player.position.z);
        GameObject partical2 = boss_Partical[1].GetItem(PlayerPosition); // 바닥에 닿았을때 생성, 2초뒤 삭제
        yield return new WaitForSeconds(5f);
        boss_Partical[1].FreeItem(partical2); // 바닥에 닿았을때 생성, 2초뒤 삭제
    }

    void Boss_Dead()
    {
        if (hp <= 0)
        {
            eState = EnemyState.Dead;
            SoundManager.instance.Boss_PlaySound(_clip[1]);
        }
    }
    void RealDead()
    {
        Destroy(gameObject);
        Time.timeScale = 0;
    }
    void RealAttack()
    {
        if (away <= 3)
        {
            Player.SendMessage("Damaged", 0.2f);
        }
        eState = EnemyState.Idle;
    }

    void Slash_Sound()
    {
        SoundManager.instance.Boss_PlaySound(_clip[0]);
    }

    void Spawn_Meteor_Sound()
    {
    }

    void CircleMeteor_Sound()
    {
    }
}
