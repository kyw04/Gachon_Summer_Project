using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class Stage2_Boss : BattleableComponentBase
{
    Animator anim;
    NavMeshAgent agent;

    [Header("����ü�� ���� UI")]
    public GameObject hp_Bar_gameobj;
    public GameObject hp_Name_gameobj;
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
    public GameObject[] cast_Effect;

    [Header("���� �������� ���� ������")]
    [SerializeField] GameObject lamp;
    [SerializeField] GameObject directional_light;
    [SerializeField] GameObject boss_torch;

    [Header("���� ����")]
    [SerializeField] private AudioClip[] _clip;
    [SerializeField] private AudioClip[] _bgmClip;

    [Header("������ �׾����� ���� ����")]
    public GameObject die_boss_camera;
    public GameObject gameClear_popup;
    public GameObject gameOver_popup;

    public float away;
    public Transform Player;
    public Boss_Line boss_Line;

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
        base.Awake();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        BtnManager.instance.sceneNum = 3;
        fillImage = hp_Bar.fillRect.GetComponentInChildren<Image>();
        Player = FindObjectOfType<PlayerComponent>().transform;

        Status = new BattleableVOBase()
        {
            maxHealthPoint = Status.maxHealthPoint,
            attackPoint = Status.attackPoint,
        };

        eState = EnemyState.Idle;
        boss_Line.isArrive_Boss = false;

        SoundManager.instance.BGM_Sound(0);
        //  SoundManager.instance.BGM_PlaySound(_bgmClip[0]);
    }


    public override int ModifyHealthPoint(int amount)
    {
        //ȸ�� �̸� 1 �°� ������ 0 ������ -1
        Damaged(-amount);
        return 0;
    }

    void Update()
    {
        //Debug.Log(boss_Line.isArrive_Boss);
        away = Vector3.Distance(transform.position, Player.position); // ���� �÷��̾��� �Ÿ�
        Vector3 playerPosition = Player.position; // �÷��̾��� ��ġ

        //  Debug.Log(away);

        if (boss_Line.isArrive_Boss && boss_Line.endProduction)
        {
            ChangeEnemyState();
        }
    }

    void Damaged(float damage) // �÷��̾ ���������� �� �Լ� ȣ�� 
    {
        Debug.Log(damage);
        hp_Bar.value -= damage;
        if (hp_Bar.value <= 0f)
        {
            hp -= 1;
            hp_T.text = "X" + hp.ToString();
            hp_Bar.value = 100f;
            // 9-1/3 = 3 
            fillImage.color = hp_Bar_Change_Color[(hp - 1) / hp_Bar_Change_Color.Length];
            if (hp <= 0)
            {
                hp = 0;
                hp_Bar.value = 0f;
                hp_T.text = "X" + hp.ToString();
                Boss_Dead();
            }
        }
    }
    // ���� ���� Coll�� �������, Ȱ��ȭ�Ǵ� �Լ� Boss_Line���� SendMassage��
    void Health_Bar()
    {
        hp_Bar_gameobj.SetActive(true);
        hp_Name_gameobj.SetActive(true);
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
        die_boss_camera.SetActive(true);
        anim.SetTrigger("dead");
    }
    void Attack()
    {
        if (delay_Time <= Time.time)
        {
            delay_Time = Time.time + 3f;

            int attackNum = Random.Range(1, 5);
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
                case 4:
                    Dark();
                    break;

            }
        }
    }



    #region ���� ����
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
    }

    public void Dark() // �Ȱ� �Ѹ���
    {
        anim.SetTrigger("dark");
    }
    void SpawnMeteor_Attack() // ������ ���׿� ����
    {
        anim.SetTrigger("spawn_Meteor");
        //Debug.Log("sky���׿�");
    }
    #endregion
    void Off_Speed()
    {
        agent.speed = 0;
        //  eState = EnemyState.Idle;
    }

    void On_Speed()
    {
        agent.destination = Player.position;     // ����ؼ� �÷��̾ ���� �̵��ϵ��� ������ ���� (destination)
        agent.isStopped = false;
        agent.speed = 3.5f;
    }
    // ���׿� ������ ������ ��������� ����

    IEnumerator Recover_dark_attack()
    {
        yield return new WaitForSeconds(3.5f);
        directional_light.transform.Rotate(new Vector3(30f, transform.rotation.y, transform.rotation.z));
        boss_torch.SetActive(true);
        lamp.SetActive(true);
    }

    #region �ִϸ��̼� �̺�Ʈ �ý��ۿ� �־��� ��¥ ����
    void Real_dark_attack()
    {
        directional_light.transform.Rotate(new Vector3(-30f, transform.rotation.y, transform.rotation.z));
        boss_torch.SetActive(false);
        lamp.SetActive(false);
        StartCoroutine(Recover_dark_attack());
    }

    void Real_CircleMeteor_Attack()
    {
        for (int i = 0; i < spawnPoint.Length; i++)
        {
            //Debug.Log("Real_CircleMeteor_Attack ����");
            GameObject meteor0 = boss_Attack[1].GetItem(spawnPoint[i].position);
            Vector3 direction = spawnPoint[i].position - transform.position; // ���� ��ġ���� spawnPoint ��ġ�� ���ϴ� ����
            direction.y = 0f;
            meteor0.GetComponent<Rigidbody>().velocity = direction.normalized * meteorSpeed;
        }
    }


    IEnumerator Destroy_Partical()
    {
        yield return new WaitForSeconds(3f);
        Vector3 PlayerPosition = new Vector3(Player.position.x, Player.position.y - 5f, Player.position.z);
        GameObject partical2 = boss_Partical[1].GetItem(PlayerPosition); // �ٴڿ� ������� ����, 2�ʵ� ����
        yield return new WaitForSeconds(5f);
        boss_Partical[1].FreeItem(partical2); // �ٴڿ� ������� ����, 2�ʵ� ����
    }

    void Real_SpawnMeteor_Attack()
    {
        Vector3 meteorPosition = new Vector3(Player.position.x,
            Player.position.y + 10f, Player.position.z);

        // ���׿��� �÷��̾��� ���� ��ġ�� ����
        GameObject meteor = boss_Attack[0].GetItem(meteorPosition);
        GameObject partical1 = boss_Partical[0].GetItem(meteorPosition);

        boss_Attack[0].FreeItem(meteor, 2f);
        boss_Partical[0].FreeItem(partical1, 2f);
        eState = EnemyState.Idle;
    }
    #endregion

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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void RealAttack()
    {
        if (away <= 3)
        {
            Player.SendMessage("Damaged", 20f);
        }
        eState = EnemyState.Idle;
    }
    void Cast_Dark_Attack()
    {
        cast_Effect[1].SetActive(true);
        SoundManager.instance.Boss_PlaySound(_clip[3]);
    }
    #region ���� ���� ���
    void Slash_Sound()
    {
        SoundManager.instance.Boss_PlaySound(_clip[0]);
    }

    void Spawn_Meteor_Sound()
    {
        SoundManager.instance.Boss_PlaySound(_clip[2]);
    }

    void Cast_CircleMeteor_Attack()
    {
        cast_Effect[0].SetActive(true);
        SoundManager.instance.Boss_PlaySound(_clip[3]);
    }
    void CircleMeteor_Sound()
    {
        cast_Effect[0].SetActive(false);
        SoundManager.instance.Boss_PlaySound(_clip[4]);
    }

    void Cast_Dark_Sound()
    {
        cast_Effect[1].SetActive(false);
        SoundManager.instance.Boss_PlaySound(_clip[5]);
    }

    public override void Move()
    {
        //   throw new System.NotImplementedException();
    }

    protected override void OnCollisionEnter(Collision other)
    {
        //    throw new System.NotImplementedException();
    }

    protected override void OnCollisionStay(Collision other)
    {
        //  throw new System.NotImplementedException();
    }

    public override void AnimEvt(string cmd)
    {
        //throw new System.NotImplementedException();
    }
    #endregion

}
