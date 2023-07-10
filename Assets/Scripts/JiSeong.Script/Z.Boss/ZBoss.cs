using UnityEngine;
using System.Collections;

public class ZBoss : MonoBehaviour
{
    public Transform target; // �÷��̾��� Transform ������Ʈ
    public GameObject prefab;

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
    }

    private void Update()
    {
        if (target != null)
        {
            if (EnemyHp > 0)
            {
                // �÷��̾� ���� ���ϱ�
                Vector3 direction = target.position - transform.position;
                direction.y = 0; // ���� �������� ���� (y�� ȸ�� ����)
                direction.Normalize(); // ���͸� ���� ���ͷ� ����ȭ

                // ���� �÷��̾� ������ �Ÿ� ���
                float distance = Vector3.Distance(transform.position, target.position);

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

                        // �� ȸ��
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                        // �ִϸ��̼� ���
                        if (animator != null)
                        {
                            animator.SetTrigger("isWalk"); // MoveSpeed �Ķ���� ����
                        }
                    }
                }
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("mixamo_com"))
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // ���� �������� ���� (y�� ȸ�� ����)
            direction.Normalize(); // ���͸� ���� ���ͷ� ����ȭ
            animator.ResetTrigger("isAttack1");
            transform.position -= direction * moveSpeed * Time.deltaTime;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Mutant Jump Attack"))
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // ���� �������� ���� (y�� ȸ�� ����)
            direction.Normalize(); // ���͸� ���� ���ͷ� ����ȭ
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

        // ù ��° ���� �����ϴ� �ڵ� �ۼ�
        // ���� ���, ������ Ư�� ���� �ִϸ��̼��� ����ϰų� �÷��̾�� �������� ���� �� �ֽ��ϴ�.
        animator.SetTrigger("isAttack1");

        yield return new WaitForSeconds(1f); // ���� ���� �ð�

        isAttacking = false;
        canAttack = false;
        isAttacking = true;
        Invoke("ResetAttack", attackCooldown);
    }

    private IEnumerator Attack2()
    {
        isAttacking = true;

        // �� ��° ���� �����ϴ� �ڵ� �ۼ�
        // ���� ���, ������ Ư�� ���� ������ �����ϰų� �ֺ��� ������ ����ų �� �ֽ��ϴ�.

        animator.SetTrigger("isAttack2");


        isAttacking = false;
        canAttack = false;
        isAttacking = true;
        Invoke("ResetAttack", attackCooldown);

        yield return new WaitForSeconds(1.8f); // ���� ���� �ð�

        SpawnPrefab();
    }
    private void SpawnPrefab()
    {
        // �������� �����Ͽ� �ν��Ͻ�ȭ�մϴ�.
        GameObject spawnedPrefab = Instantiate(prefab, transform.position, Quaternion.identity);

        // 2�� �Ŀ� �������� �����ϴ� �Լ� ȣ���� �����մϴ�.
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
        // ���� ������Ʈ�� ��ġ�� ������
        Vector3 currentPosition = transform.position;

        // y ��ǥ ���� ���ҽ�Ŵ
        currentPosition.y -= 0.6f;

        // ��ġ�� �����Ͽ� ������Ʈ�� y ��ǥ ���� ����
        transform.position = currentPosition;
    }
    private System.Collections.IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(5f);

        // 4�� �ڿ� ������Ʈ ������� �ϱ�
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� �߻��� ���
        if (collision.gameObject.CompareTag("Player"))
        {

            ignoreCollisionTimer = ignoreCollisionDuration;
            StartCoroutine(ResetIgnoreCollisionTimer());
        }
    }
    private System.Collections.IEnumerator ResetIgnoreCollisionTimer()
    {
        // HP�� ���ҽ�Ŵ
        EnemyHp--;

        if (EnemyHp <= 0)
        {
            transform.rotation = Quaternion.Euler(0, -0.4f, 0);
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // ���� �������� ���� (y�� ȸ�� ����)
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
