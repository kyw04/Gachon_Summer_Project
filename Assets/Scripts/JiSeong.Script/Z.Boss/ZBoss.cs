using UnityEngine;
using System.Collections;

public class ZBoss : MonoBehaviour
{
    public Transform target; // 플레이어의 Transform 컴포넌트
    public GameObject prefab;

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
    }

    private void Update()
    {
        if (target != null)
        {
            if (EnemyHp > 0)
            {
                // 플레이어 방향 구하기
                Vector3 direction = target.position - transform.position;
                direction.y = 0; // 수평 방향으로 제한 (y축 회전 방지)
                direction.Normalize(); // 벡터를 단위 벡터로 정규화

                // 적과 플레이어 사이의 거리 계산
                float distance = Vector3.Distance(transform.position, target.position);

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

                        // 적 회전
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                        // 애니메이션 재생
                        if (animator != null)
                        {
                            animator.SetTrigger("isWalk"); // MoveSpeed 파라미터 설정
                        }
                    }
                }
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("mixamo_com"))
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // 수평 방향으로 제한 (y축 회전 방지)
            direction.Normalize(); // 벡터를 단위 벡터로 정규화
            animator.ResetTrigger("isAttack1");
            transform.position -= direction * moveSpeed * Time.deltaTime;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Mutant Jump Attack"))
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // 수평 방향으로 제한 (y축 회전 방지)
            direction.Normalize(); // 벡터를 단위 벡터로 정규화
            animator.ResetTrigger("isAttack2");
            transform.position -= direction * moveSpeed * Time.deltaTime;
        }
        if (ignoreCollisionTimer > 0f)
        {
            ignoreCollisionTimer -= Time.deltaTime;
        }
    }
    private void PerformRandomAttack()
    {
        float distance = Vector3.Distance(transform.position, target.position);
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

        // 첫 번째 공격 실행하는 코드 작성
        // 예를 들어, 보스가 특정 공격 애니메이션을 재생하거나 플레이어에게 데미지를 입힐 수 있습니다.
        animator.SetTrigger("isAttack1");

        yield return new WaitForSeconds(1f); // 공격 지속 시간

        isAttacking = false;
        canAttack = false;
        isAttacking = true;
        Invoke("ResetAttack", attackCooldown);
    }

    private IEnumerator Attack2()
    {
        isAttacking = true;

        // 두 번째 공격 실행하는 코드 작성
        // 예를 들어, 보스가 특정 공격 패턴을 수행하거나 주변에 폭발을 일으킬 수 있습니다.

        animator.SetTrigger("isAttack2");


        isAttacking = false;
        canAttack = false;
        isAttacking = true;
        Invoke("ResetAttack", attackCooldown);

        yield return new WaitForSeconds(1.8f); // 공격 지속 시간

        SpawnPrefab();
    }
    private void SpawnPrefab()
    {
        // 프리팹을 생성하여 인스턴스화합니다.
        GameObject spawnedPrefab = Instantiate(prefab, transform.position, Quaternion.identity);

        // 2초 후에 프리팹을 삭제하는 함수 호출을 예약합니다.
        Destroy(spawnedPrefab, 2f);
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
        // 현재 오브젝트의 위치를 가져옴
        Vector3 currentPosition = transform.position;

        // y 좌표 값을 감소시킴
        currentPosition.y -= 0.6f;

        // 위치를 변경하여 오브젝트의 y 좌표 값을 줄임
        transform.position = currentPosition;
    }
    private System.Collections.IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(5f);

        // 4초 뒤에 오브젝트 사라지게 하기
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌이 발생한 경우
        if (collision.gameObject.CompareTag("Player"))
        {

            ignoreCollisionTimer = ignoreCollisionDuration;
            StartCoroutine(ResetIgnoreCollisionTimer());
        }
    }
    private System.Collections.IEnumerator ResetIgnoreCollisionTimer()
    {
        // HP를 감소시킴
        EnemyHp--;

        if (EnemyHp <= 0)
        {
            transform.rotation = Quaternion.Euler(0, -0.4f, 0);
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // 수평 방향으로 제한 (y축 회전 방지)
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
