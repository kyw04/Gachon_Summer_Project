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

    [Header("�������� �����ϴ� ������ ���� ������")]
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
        away = Vector3.Distance(transform.position, Player.position); // ���� �÷��̾��� �Ÿ�
        Vector3 playerPosition = Player.position; // �÷��̾��� ��ġ

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
        agent.isStopped = true; // ��ã�� ����
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
        agent.destination = Player.position;     // ����ؼ� �÷��̾ ���� �̵��ϵ��� ������ ���� (destination)
        agent.isStopped = false; // ��ã�� ����
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
    public void CircleMeteorAttack() // �������� ���׿� ����
    {
        anim.SetTrigger("shootMeteor");
        Debug.Log("���� ���׿�");
    }

    void Real_CircleMeteor_Attack()
    {
        for (int i = 0; i < spawnPoint.Length; i++)
        {
            Debug.Log("Real_CircleMeteor_Attack ����");
            GameObject meteor0 = boss_Attack[1].GetItem(spawnPoint[i].position);
            Vector3 direction = spawnPoint[i].position - transform.position; // ���� ��ġ���� spawnPoint ��ġ�� ���ϴ� ����
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


    void SpawnMeteor_Attack() // ������ ���׿� ����
    {
        anim.SetTrigger("spawn_Meteor");
        Debug.Log("sky���׿�");
    }

    void Real_SpawnMeteor_Attack()
    {
        //    Debug.Log("Real_SpawnMeteor_Attack ����");
        // ���׿��� �÷��̾��� ���� ��ġ�� ����
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
