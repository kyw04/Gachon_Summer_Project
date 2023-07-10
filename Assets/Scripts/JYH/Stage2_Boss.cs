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

    [Header("����ü�� ���� UI")]
    public Slider hp_Bar;
    public int hp;
    [SerializeField] Text hp_T;
    Image fillImage;
    [SerializeField] Color32[] hp_Bar_Change_Color;


    Rigidbody rb;

    [Header("�������� �����ϴ� ������ ���� ������")]
    public float meteorSpeed;
    [SerializeField] Transform[] spawnPoint;


    public ObjectPoolComponent[] boss_Attack;
    public ObjectPoolComponent[] boss_Partical;
    public GameObject die_Effect;


    [Header("���� ����")]
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
        away = Vector3.Distance(transform.position, Player.position); // ���� �÷��̾��� �Ÿ�
        Vector3 playerPosition = Player.position; // �÷��̾��� ��ġ

        //  Debug.Log(away);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Damaged(0.2f);
        }

        ChangeEnemyState();
    }
    void Damaged(float damage) // �÷��̾ ���������� �� �Լ� ȣ�� 
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

    /*void RealAttack() // �÷��̾ ����������: �÷��̾� ��ũ��Ʈ�� ���� ������ �ڵ�
    {
        if (away<=2.5)
        {
             Enemy.SendMessage("Damaged", ���� ������);
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
        {
            anim.SetTrigger("slash");
        }
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
        GameObject partical1 = boss_Partical[0].GetItem(meteorPosition);

        boss_Attack[0].FreeItem(meteor, 2f);
        boss_Partical[0].FreeItem(partical1, 2f);


        eState = EnemyState.Idle;

    }

    IEnumerator Destroy_Partical()
    {
        yield return new WaitForSeconds(3f);
        Vector3 PlayerPosition = new Vector3(Player.position.x, Player.position.y - 5f, Player.position.z);
        GameObject partical2 = boss_Partical[1].GetItem(PlayerPosition); // �ٴڿ� ������� ����, 2�ʵ� ����
        yield return new WaitForSeconds(5f);
        boss_Partical[1].FreeItem(partical2); // �ٴڿ� ������� ����, 2�ʵ� ����
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
