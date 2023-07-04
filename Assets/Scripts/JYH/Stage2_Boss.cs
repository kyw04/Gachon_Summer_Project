using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Stage2_Boss : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    [SerializeField] int Boss_AttackNum;

    [SerializeField] int ranNum;

    public ObjectPoolComponent boss_Attack;
    Transform playerTransform;
    IEnumerator attack;

    Vector3 dir;

    public float dirX;
    public float dirZ;

    public enum EnemyState
    {
        idle,
        Walk,
        Attack,
        Damaged,
        Dead
    }
    public EnemyState eState;


    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        eState = EnemyState.Walk;

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        attack = Attack_C();
        StartCoroutine(attack);
        StartCoroutine(Move_C());


        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            StopCoroutine(attack);

        ChangeEnemyState();

        dir = Vector3.Lerp(dir, new Vector3(dirX , 0, dirZ), Time.deltaTime * 5);
        anim.SetFloat("Z", dir.z);
        anim.SetFloat("X", dir.x);
    }

    void ChangeEnemyState()
    {
        switch (eState)
        {
            case EnemyState.Walk:
                Walk(dir);
                break;

            case EnemyState.Attack:
                break;

            case EnemyState.Dead:
                break;
        }
    }

    public void Walk(Vector3 dir)
    {
        
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
                    Vector3 playerPosition = playerTransform.position;
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
