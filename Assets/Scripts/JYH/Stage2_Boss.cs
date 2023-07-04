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
                // 낫 한번 휘두르기 
                case 1:

                    break;
                // 낫 두번 휘두르기 
                case 2:

                    break;
                // 마법 공격(메테오 소환)
                case 3:
                    // 플레이어의 위치를 얻어옴
                    Vector3 playerPosition = playerTransform.position;
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
