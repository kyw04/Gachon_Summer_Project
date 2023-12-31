using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dragon : MonoBehaviour
{
    public enum State
    {
        None,
        Idle,
        Attack,
        Move
    }

    public GameObject player;
    public ParticleSystem[] particles;
    public ParticleSystem[] fireParticles;
    public NavMeshAgent nvAgent;

    #region Position

    [Header("Position")]
    public Transform[] hands;
    public Transform mouth;
    public Transform chest;
    public Transform dash;

    #endregion
    #region Stats

    [Header("Stats")]
    public Vector3 handAttackBox;
    public Vector3 mouthAttackBox;
    public float movementSpeed = 5f;
    public float damage = 10f;
    public float attackRandMax = 1;
    public float attackDlay = 5;
    private float attackTime;

    [Space(15)]
    public float screamDistance = 30f;
    public float screamPower = 0.5f;
    public float dashPower = 50f;
    public float dashTime = 1f;
    #endregion
    #region Dictionary
    [Header("Dictionary")]
    public string[] key;
    public ObjectPoolComponent[] value;
    private Dictionary<string, ObjectPoolComponent> objectPools = new Dictionary<string, ObjectPoolComponent>();

    #endregion

    private State state;
    private Animator animator;
    private Rigidbody rigi;
    private Quaternion idleStateRotation;
    private float timeSpeed = 0.5f;
    private float wingSpeed = 1f;
    private bool isSleeping;
    private bool isScreaming;
    private bool isWingAttacking;
    private bool isFly;

    private Transform attackTransform;
    private Vector3 attackPos;
    private Vector3 attackBoxSize;
    private Quaternion attackBoxRotation;
    private float attackRadius;

    private void Awake()
    {
        for (int i = 0; i < key.Length; i++)
        {
            try
            {
                objectPools.Add(key[i], value[i]);
            }
            catch
            {
                Debug.Log("missing match");
            }
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody>();
        nvAgent = GetComponent<NavMeshAgent>();

        state = State.Idle;
        isSleeping = true;
        isScreaming = false;
        isWingAttacking = false;
        isFly = false;
    }

    private void Update()
    {
        if (state == State.Idle)
        {
            foreach (ParticleSystem particle in particles) { particle.Stop(); }
            foreach (ParticleSystem particle in fireParticles) { particle.Stop(); }
            idleStateRotation = transform.rotation;
            attackTransform = null;
            attackPos = Vector3.zero;

            if (attackTime + attackDlay <= Time.time)
            {
                attackTime = Time.time + Random.Range(0, attackRandMax);

                float dis = Vector3.Distance(player.transform.position, transform.position);
                int num;
                if (dis <= 10)
                {
                    num = Random.Range(0, 5);
                    if (num == 1)
                    {
                        BackDash();
                        LaterAttack("AttackFlame", 1f);
                    }
                    else if (num == 2)
                    {
                        BackDash();
                        LaterAttack("AttackHand", 1f);
                    }
                }
                else
                {
                    num = Random.Range(1, 3);
                }

                Debug.Log($"{attackTime}\ndis: {dis} {num}");
                switch (num)
                {
                    case 0:
                        Attack("AttackMouth", false, mouth, mouthAttackBox);
                        break;
                    case 1:
                        Attack("AttackFlame");
                        break;
                    case 2:
                        Attack("AttackHand");
                        break;
                    case 3:
                        Attack("AttackScream");
                        break;
                    case 4:
                        Attack("AttackMagic", true, transform, 15f);
                        break;
                    case 5:
                        Attack("AttackWing", true);
                        isWingAttacking = true;
                        foreach (ParticleSystem particle in fireParticles) { particle.Play(); }
                        break;
                    default:
                        break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
            if (isFly)
                Land();
            else
                Fly();

        WakeUp();
        AttackWings();

        if (isScreaming)
            Scream();

        if (state == State.Attack)
        {
            Vector3 pos = attackTransform ? attackTransform.position : attackPos;

            Collider[] colliders = Physics.OverlapBox(pos, attackBoxSize, attackBoxRotation, LayerMask.GetMask("Player"));
            foreach (Collider collider in colliders)
            {
                Debug.Log(collider.name);
            }
        }
    }

    private void WakeUp()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp"))
            return;

        if (!isSleeping)
        {
            float wakeValue = animator.GetFloat("Wake");
            if (wakeValue >= 0.3f && wakeValue <= 0.8f)
            {
                Scream();
            }

            animator.SetFloat("Wake", wakeValue + Time.deltaTime * timeSpeed);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) <= 20f)
        {
            isSleeping = false;
            animator.SetTrigger("WakeUp");
        }
    }

    private void Attack(string aniName, bool bodyCollision, Transform pos, Vector3 size)
    {
        if (state != State.Idle)
            return;

        Attack(aniName, bodyCollision);
        attackTransform = pos;
        attackBoxSize = size;
        attackRadius = 0f;
    }
    private void Attack(string aniName, bool bodyCollision, Transform pos, float radius)
    {
        if (state != State.Idle)
            return;

        Attack(aniName, bodyCollision);
        attackTransform = pos;
        attackBoxSize = Vector3.zero;
        attackRadius = radius;
    }
    private void Attack(string aniName, bool bodyCollision)
    {
        if (state != State.Idle)
            return;

        Attack(aniName);
    }
    private void Attack(string aniName)
    {
        if (state != State.Idle)
            return;

        state = State.Attack;
        attackTransform = null;
        attackPos = Vector3.zero;
        attackBoxRotation = transform.rotation;
        attackBoxSize = Vector3.zero;
        attackRadius = 0;
        animator.ResetTrigger("BackDash");

        animator.SetTrigger(aniName);
    }
    private void AttackFireBall(string poolNmae)
    {
        Attack("AttackFireBall");
        objectPools[poolNmae].GetItem(mouth.position);
    }
    private void AttackWings()
    {
        if (!isWingAttacking)
            return;

        float wingTime = animator.GetFloat("WingAttackTime") + wingSpeed * timeSpeed * Time.deltaTime;
        animator.SetFloat("WingAttackTime", wingTime);

        if (wingTime >= 1)
            wingSpeed = -1;
        if (wingTime <= 0)
        {
            isWingAttacking = false;
            wingSpeed = 1;
            transform.rotation = idleStateRotation;
            animator.SetFloat("WingAttackTime", 0f);
            rigi.velocity = Vector3.zero;
            return;
        }

        if (wingSpeed > 0 && wingTime <= 0.55f)
        {
            if (wingTime >= 0.5f)
            {
                wingTime += 0.05f;
                animator.SetFloat("WingAttackTime", wingTime);
                rigi.velocity = Vector3.zero;
                rigi.AddForce(transform.forward * 50000f, ForceMode.Force);
            }
            else
            {
                transform.position -= transform.forward * movementSpeed * Time.deltaTime;
                transform.Rotate(new Vector3(15 * Time.deltaTime, 0, 0));
            }
        }
        else if (wingSpeed < 0 && wingTime <= 0.5f)
        {
            transform.Rotate(new Vector3(-15 * Time.deltaTime, 0, 0));
            state = State.Idle;
        }
    }
    private void Scream()
    {
        //Debug.Log("scream");
        Collider[] colliders = Physics.OverlapSphere(transform.position, screamDistance);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Rigidbody>())
            {
                Vector3 force = (collider.transform.position - transform.position).normalized;
                collider.GetComponent<Rigidbody>().AddForce(force * screamPower, ForceMode.Impulse);
            }
        }
    }
    private void HandAttack(string poolNmae)
    {
        GameObject leftParticle = objectPools[poolNmae].GetItem(hands[0].position + Vector3.up * 0.125f);
        GameObject rightParticle = objectPools[poolNmae].GetItem(hands[1].position + Vector3.up * 0.125f);

        leftParticle.transform.LookAt(transform.forward * 100);
        rightParticle.transform.LookAt(transform.forward * 100);

        attackBoxSize = handAttackBox;
        attackTransform = null;
        Vector3 pos = leftParticle.transform.position;
        pos += leftParticle.transform.forward * attackBoxSize.z * 0.5f;
        pos.x += Vector3.Distance(leftParticle.transform.position, rightParticle.transform.position) * 0.5f;
        attackPos = pos;
        attackBoxRotation = transform.rotation;

        objectPools[poolNmae].FreeItem(leftParticle, 1.5f);
        objectPools[poolNmae].FreeItem(rightParticle, 1.5f);
    }
    private void MagicAttack(string poolNmae)
    {
        GameObject newParticle = objectPools[poolNmae].GetItem(transform);

        objectPools[poolNmae].FreeItem(newParticle, 1.5f);
    }
    private void OnScream(float time)
    {
        isScreaming = true;
        Invoke("OffScream", time);
    }
    private void OffScream()
    {
        isScreaming = false;
    }
    private void Fly()
    {
        if (state != State.Idle)
            return;

        state = State.Move;
        attackTransform = null;
        attackPos = Vector3.zero;
        attackBoxRotation = transform.rotation;
        attackBoxSize = Vector3.zero;
        attackRadius = 0;

        isFly = true;
        //rigi.useGravity = false;
        animator.SetTrigger("TakeOff");
    }
    private void Land()
    {
        if (state != State.Idle)
            return;

        state = State.Move;
        attackTransform = null;
        attackPos = Vector3.zero;
        attackBoxRotation = transform.rotation;
        attackBoxSize = Vector3.zero;
        attackRadius = 0;

        isFly = false;
        //rigi.useGravity = true;
        animator.SetTrigger("Land");
    }
    private void BackDash()
    {
        if (state != State.Idle)
            return;

        StartCoroutine(Dash());
        animator.SetTrigger("BackDash");
    }

    private IEnumerator Dash()
    {
        Debug.Log("Dash");
        Vector3 finishPos = dash.position;
        rigi.useGravity = false;

        float currentTime = Time.time;
        float currentDashTime = Time.time + dashTime;

        while (currentTime < currentDashTime)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Slerp(transform.position, finishPos, dashPower * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        rigi.useGravity = true;
        Debug.Log("Dash Finish");
    }
    private IEnumerator LaterAttack(string aniName, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Attack(aniName);
        attackTime += seconds;
    }
    private void SetIdle() { state = State.Idle; Debug.Log(state); }
    private void ParticlePlay(int index) { particles[index].Play(); }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, screamDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 20f);

        if (state == State.Attack)
        {
            Vector3 pos = attackTransform ? attackTransform.position : attackPos;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(pos, attackBoxSize);
            Gizmos.DrawWireSphere(pos, attackRadius);
        }
    }
}
