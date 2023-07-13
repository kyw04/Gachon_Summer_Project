using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public enum State
    {
        None,
        Idle,
        Attack,
        Walk
    }

    public GameObject player;
    public ParticleSystem[] particles;
    public Transform[] hands;
    public Transform mouth;

    public float screamDistance = 30;
    public float screamPower = 0.5f;

    [Header("Stats")]
    public float movementSpeed = 5f;
    public float damage = 10f;

    private State state;
    private Animator animator;
    private float wakeUpSpeed = 0.5f;
    private bool isSleeping;
    private bool isScreaming;

    [Header("Dictionary")]
    public string[] key;
    public ObjectPoolComponent[] value;
    private Dictionary<string, ObjectPoolComponent> objectPools = new Dictionary<string, ObjectPoolComponent>();

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
        state = State.Idle;
        isSleeping = true;
        isScreaming = false;
    }

    private void Update()
    {
        if (state == State.Idle)
        {
            foreach (ParticleSystem particle in particles)
                particle.Stop();
        }

        WakeUp();

        if (isScreaming)
            Scream();

        if (Input.GetKeyDown(KeyCode.Q))
            Attack("AttackHand");
        if (Input.GetKeyDown(KeyCode.W))
            Attack("AttackMouth");
        if (Input.GetKeyDown(KeyCode.E))
            Attack("AttackFlame");
        if (Input.GetKeyDown(KeyCode.R))
            Attack("AttackScream");
        if (Input.GetKeyDown(KeyCode.T))
            Attack("TakeOff");
        if (Input.GetKeyDown(KeyCode.Y))
            Attack("AttackWing");
        if (Input.GetKeyDown(KeyCode.A))
            Attack("AttackMagic");
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

            animator.SetFloat("Wake", wakeValue + Time.deltaTime * wakeUpSpeed);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) <= 20f)
        {
            isSleeping = false;
            animator.SetTrigger("WakeUp");
        }
    }

    private void Attack(string aniName)
    {
        if (state != State.Idle)
            return;

        state = State.Attack;
        animator.SetTrigger(aniName);
    }

    private void AttackFireBall(string poolNmae)
    {
        Attack("AttackFireBall");
        objectPools[poolNmae].GetItem(mouth.position);
    }
    private void AttackWing()
    {

    }
    private void Scream()
    {
        Debug.Log("scream");
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
    private void SetIdle() { state = State.Idle; Debug.Log(state); }
    private void ParticlePlay(int index) { particles[index].Play(); }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, screamDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 20f);
    }
}
