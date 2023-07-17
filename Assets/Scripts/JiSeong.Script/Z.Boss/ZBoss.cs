using UnityEngine;
using System.Collections;

public class ZBoss : MonoBehaviour
{
    public GameObject target; // �÷��̾��� Transform ������Ʈ
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject attackObjectPrefab; // �߻��� ���� ������Ʈ ������
    public float attackRadius = 5f; // ���� �ݰ�

    public float moveSpeed = 5f; // ���� �̵� �ӵ�
    public float rotationSpeed = 5f; // ȸ�� �ӵ�
    private bool ritationColldown = false; // ���� �� ȸ�� ����
    public float attackRange = 2f; // ���� ����
    public float attackDamage = 10f; // ���ݷ�
    public float attackCooldown = 2f; // ���� ��ٿ�
    public float pushForce = 5f; // �浹�� ���� �и��� ��
    public int EnemyHp = 3;
    public float ignoreCollisionDuration = 3f; // �浹 ���� �Ⱓ
    private bool isChasing = true; // ������ �÷��̾ �Ѵ��� ���θ� ��Ÿ���� ����

    private Animator animator; // Animator ������Ʈ
    private bool canAttack = true; // ���� ���� ����
    private bool isAttacking = false; // ���� �� ����
    private Rigidbody rb; // Rigidbody ������Ʈ
    private float ignoreCollisionTimer; // �浹 ���� Ÿ�̸�
    private bool isDying = false;
    private Renderer objectRenderer;

    private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");
    private static readonly int HitTriggerHash = Animator.StringToHash("Hit");

    private void Start()
    {
        animator = GetComponent<Animator>(); // Animator ������Ʈ ��������
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
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
                // �÷��̾� ���� ���ϱ�
                Vector3 direction = target.transform.position - transform.position;
                direction.y = 0; // ���� �������� ���� (y�� ȸ�� ����)
                direction.Normalize(); // ���͸� ���� ���ͷ� ����ȭ

                // ���� �÷��̾� ������ �Ÿ� ���
                float distance = Vector3.Distance(transform.position, target.transform.position);

                if (distance <= attackRange && canAttack && !isAttacking)
                {
                    // �÷��̾ ���� ���� ���� �ְ� ���� �����ϸ� ���� ���� �ƴ� ���
                    PerformRandomAttack();
                }
                else
                {
                    if (distance >= attackRange)
                    {
                        // �� �̵�
                        transform.position += direction * moveSpeed * Time.deltaTime;

                        if (ritationColldown == false)
                        {
                            // �� ȸ��
                            Quaternion targetRotation = Quaternion.LookRotation(direction);
                            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                        }

                        // �ִϸ��̼� ���
                        if (animator != null)
                        {
                            animator.SetTrigger("isWalk"); // MoveSpeed �Ķ���� ����
                        }
                    }
                }
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Attack (2)"))
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0; // ���� �������� ���� (y�� ȸ�� ����)
            direction.Normalize(); // ���͸� ���� ���ͷ� ����ȭ
            animator.ResetTrigger("isAttack1");
            transform.position -= direction * moveSpeed * Time.deltaTime;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Mutant Jump Attack"))
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0; // ���� �������� ���� (y�� ȸ�� ����)
            direction.Normalize(); // ���͸� ���� ���ͷ� ����ȭ
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
                int attackType = Random.Range(1, 3); // 1 �Ǵ� 2 �߿��� ������ ���� ����

                // ������ ���� ����
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
        // �������� �����Ͽ� �ν��Ͻ�ȭ�մϴ�.
        GameObject spawnedPrefab = Instantiate(prefab2, transform.position, Quaternion.identity);

        // 2�� �Ŀ� �������� �����ϴ� �Լ� ȣ���� �����մϴ�.
        Destroy(spawnedPrefab, 0.7f);
    }
    private void SpawnPrefab1()
    {
        // �������� �����Ͽ� �ν��Ͻ�ȭ�մϴ�.
        GameObject spawnedPrefab = Instantiate(prefab1, transform.position, transform.rotation);

        // 2�� �Ŀ� �������� �����ϴ� �Լ� ȣ���� �����մϴ�.
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

        // 4�� �ڿ� ������Ʈ ������� �ϱ�
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
