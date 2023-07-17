using UnityEngine;
using System.Collections;

public class ZBoss : MonoBehaviour
{
    public GameObject target; // 플레이어의 Transform 컴포넌트
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject attackObjectPrefab; // 발사할 공격 오브젝트 프리팹
    public float attackRadius = 5f; // 공격 반경

    public float moveSpeed = 5f; // 적의 이동 속도
    public float rotationSpeed = 5f; // 회전 속도
    private bool ritationColldown = false; // 공격 중 회전 방지
    public float attackRange = 2f; // 공격 범위
    public float attackDamage = 10f; // 공격력
    public float attackCooldown = 2f; // 공격 쿨다운
    public float pushForce = 5f; // 충돌로 인한 밀리는 힘
    public int EnemyHp = 3;
    public float ignoreCollisionDuration = 3f; // 충돌 무시 기간
    private bool isChasing = true; // 보스가 플레이어를 쫓는지 여부를 나타내는 변수

    private Animator animator; // Animator 컴포넌트
    private bool canAttack = true; // 공격 가능 여부
    private bool isAttacking = false; // 공격 중 여부
    private Rigidbody rb; // Rigidbody 컴포넌트
    private float ignoreCollisionTimer; // 충돌 무시 타이머
    private bool isDying = false;
    private Renderer objectRenderer;

    private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");
    private static readonly int HitTriggerHash = Animator.StringToHash("Hit");

    private void Start()
    {
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
        objectRenderer = GetComponent<Renderer>();
        InvokeRepeating("PerformRandomAttack", attackCooldown, attackCooldown);
        target = FindObjectOfType<PlayerComponent>().gameObject;
    }

    private void Update()
    {
        if (target != null)
        {
            if (EnemyHp > 0)
            {
                // 플레이어 방향 구하기
                Vector3 direction = target.transform.position - transform.position;
                direction.y = 0; // 수평 방향으로 제한 (y축 회전 방지)
                direction.Normalize(); // 벡터를 단위 벡터로 정규화

                // 적과 플레이어 사이의 거리 계산
                float distance = Vector3.Distance(transform.position, target.transform.position);

                if (distance <= attackRange && canAttack && !isAttacking)
                {
                    // 플레이어가 공격 범위 내에 있고 공격 가능하며 공격 중이 아닌 경우
                    PerformRandomAttack();
                }
                else
                {
                    if (distance >= attackRange)
                    {
                        // 적 이동
                        transform.position += direction * moveSpeed * Time.deltaTime;

                        if (ritationColldown == false)
                        {
                            // 적 회전
                            Quaternion targetRotation = Quaternion.LookRotation(direction);
                            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                        }

                        // 애니메이션 재생
                        if (animator != null)
                        {
                            animator.SetTrigger("isWalk"); // MoveSpeed 파라미터 설정
                        }
                    }
                }
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Attack (2)"))
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0; // 수평 방향으로 제한 (y축 회전 방지)
            direction.Normalize(); // 벡터를 단위 벡터로 정규화
            animator.ResetTrigger("isAttack1");
            transform.position -= direction * moveSpeed * Time.deltaTime;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Mutant Jump Attack"))
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0; // 수평 방향으로 제한 (y축 회전 방지)
            direction.Normalize(); // 벡터를 단위 벡터로 정규화
            animator.ResetTrigger("isAttack2");
            transform.position -= direction * moveSpeed * Time.deltaTime;
        }
        if (ignoreCollisionTimer > 0f)
        {
            ignoreCollisionTimer -= Time.deltaTime;
        }
        rb.useGravity = true;
    }
    private void PerformRandomAttack()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= attackRange)
        {
            if (isChasing && !isAttacking)
            {
                int attackType = Random.Range(1, 3); // 1 또는 2 중에서 랜덤한 공격 선택

                // 선택한 공격 실행
                switch (attackType)
                {
                    case 1:
                        StartCoroutine(Attack1());
                        break;
                    case 2:
                        StartCoroutine(Attack2());
                        break;
                }
            }
        }
    }
    private IEnumerator Attack1()
    {
        isAttacking = true;
        ritationColldown = true;
        animator.SetTrigger("isAttack1");

        isAttacking = false;
        canAttack = false;
        isAttacking = true;
        Invoke("ResetAttack", attackCooldown);
        yield return new WaitForSeconds(1.5f);

        SpawnPrefab1();

        yield return new WaitForSeconds(0.5f);

        ritationColldown = false;
        ResetAttack();
    }

    private IEnumerator Attack2()
    {
        isAttacking = true;
        ritationColldown = true;
        animator.SetTrigger("isAttack2");


        isAttacking = false;
        canAttack = false;
        isAttacking = true;
        Invoke("ResetAttack", attackCooldown);

        yield return new WaitForSeconds(1.8f); 

        SpawnPrefab2();
        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f;
            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 direction = rotation * Vector3.forward;

            Vector3 attackPosition = transform.position + direction * attackRadius;

            Instantiate(attackObjectPrefab, attackPosition, rotation);
        }

        yield return new WaitForSeconds(0.4f);

        ritationColldown = false;
        ResetAttack();
    }
    private void SpawnPrefab2()
    {
        // 프리팹을 생성하여 인스턴스화합니다.
        GameObject spawnedPrefab = Instantiate(prefab2, transform.position, Quaternion.identity);

        // 2초 후에 프리팹을 삭제하는 함수 호출을 예약합니다.
        Destroy(spawnedPrefab, 0.7f);
    }
    private void SpawnPrefab1()
    {
        // 프리팹을 생성하여 인스턴스화합니다.
        GameObject spawnedPrefab = Instantiate(prefab1, transform.position, transform.rotation);

        // 2초 후에 프리팹을 삭제하는 함수 호출을 예약합니다.
        Destroy(spawnedPrefab, 0.7f);
    }

    public void StopChasing()
    {
        isChasing = false;
    }
    public void Die()
    {
        if (!isDying)
        {
            isDying = true;
            StartCoroutine(DieCoroutine());
        }
    }
    private System.Collections.IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(5f);

        // 4초 뒤에 오브젝트 사라지게 하기
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isAttacking == false)
            {
                ignoreCollisionTimer = ignoreCollisionDuration;
                StartCoroutine(ResetIgnoreCollisionTimer());
            }
        }
    }
    private System.Collections.IEnumerator ResetIgnoreCollisionTimer()
    {
        EnemyHp--;

        if (EnemyHp <= 0)
        {
            transform.rotation = Quaternion.Euler(0, -0.4f, 0);
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0; 
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
            rb.isKinematic = true;
            animator.SetTrigger("isDie");
            Die();
        }

        yield return new WaitForSeconds(ignoreCollisionDuration);

        ignoreCollisionTimer = 0f;
    }


    private void ResetAttack()
    {
        canAttack = true;
        isAttacking = false;
    }
}
