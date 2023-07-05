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
        // �÷��̾�� �ణ ���������
        if (away <= 10)
        {
            // �̵� ���·� ��ȯ
            eState = EnemyState.Walk;
            // �̵� �ִϸ��̼����� ��ȯ
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
            // ���� ���°� ���ڸ��� ��������
            attackDelayTime = 1;
            eState = EnemyState.Attack1;
        }
        // �Ÿ��� �־����� �� �⺻ ����
        else if (away > 10)
        {
            eState = EnemyState.Idle;

            // �⺻ �ִϸ��̼����� ��ȯ
            anim.SetFloat("Z", 0);

            // ��ã�� ����
            agent.isStopped = true;
        }
        //�÷��̾ ���� �̵�
        else
        {
            Debug.Log("�÷��̾�� ������");
            anim.SetFloat("Z", 1);
            // ����ؼ� �÷��̾ ���� �̵��ϵ��� ������ ���� (destination)
            agent.destination = Player.position;
            // ��ã�� ����
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
        // ���� �ð� ����
        attackDelayTime += Time.deltaTime;

        // 1�ʰ� ������
        if (attackDelayTime >= 1)
        {
            // ���� �ִϸ��̼����� ��ȯ
            anim.SetTrigger("attack");

            // 0���� �ʱ�ȭ�ؼ� �ٽ� ó������ 1�� ����
            attackDelayTime = 0;
        }
        // �÷��̾ �־�����
        if (away > 5)
        {
            // �̵� ���·� ��ȯ
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
                // �� �ѹ� �ֵθ��� 
                case 1:

                    break;
                // �� �ι� �ֵθ��� 
                case 2:

                    break;
                // ���� ����(���׿� ��ȯ)
                case 3:
                    // �÷��̾��� ��ġ�� ����
                    Vector3 playerPosition = Player.position;
                    // ���׿��� �÷��̾��� ���� ��ġ�� ����
                    Vector3 meteorPosition = new Vector3(playerPosition.x, playerPosition.y + 3f, playerPosition.z);
                    GameObject meteor = boss_Attack.GetItem(meteorPosition);
                    meteor.GetComponent<Meteor_2Stage>().boss_Attack = this.boss_Attack;
                    //// GameObject meteor = Instantiate(meteorPrefab, meteorPosition, Quaternion.identity);
                    if (meteor.transform.position.y <= -0.5)
                        boss_Attack.FreeItem(meteor);


                    // ������ ���׿��� ���� �߰� ������ �ʿ��ϴٸ� ���⿡ �߰� �ڵ� �ۼ�
                    break;
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
