using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class PaladinComponent : BattleableComponentBase
{
    public bool isActing;
    public bool isActionable;
    public bool isDamageable = true;
    public float coolDown;

    public List<GameObject> magicInstances;

    [SerializeField]
    private PlayerComponent playerInstance;
    private float distance = 0;
    delegate void Act();
    // Start is called before the first frame update
    void Start()
    {
        Status = new BattleableVOBase()
        {
            name = Status.name,
            attackPoint = Status.attackPoint,
            maxHealthPoint = Status.maxHealthPoint,
            spd = 2
        };
        playerInstance = GameObject.FindWithTag("Player").GetComponent<PlayerComponent>();
        healthPoint = Status.maxHealthPoint;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance = Vector3.Distance(this.transform.position, playerInstance.transform.position);
        Think();
        Move();
    }

    private void Think()
    {
        if (isActing || !isActionable || healthPoint == 0) return;

        isActionable = false;
        Act action = Cast;

        if (distance <= 1.5f)
            action = () =>
            {
                isActing = true;
                animator.SetTrigger("Kick");
            };
        else if (distance <= 2.5f)
            action = Attack;
        
        StartCoroutine(CallMethodWaitForSeconds(coolDown, () => { isActionable = true;}));
        action();
    }
    
    public void Cast()
    {
        isActing = true;
        animator.SetTrigger("Cast");


        StartCoroutine(Casting());
        
    }

    public override void Attack()
    {
        base.Attack();
        isActing = true;
        animator.SetBool("isWalkingForward", false);
    }

    public override void Move()
    {
        if (isActing || this.healthPoint <= 0) return;
        this.transform.LookAt(playerInstance.transform);

        if (distance <= 3f)
        {
            Rigidbody.velocity = Vector3.zero;
            animator.SetBool("isWalkingForward", false);
            animator.SetBool("isIdle", true);
        }
        else
        {
            lookFoward = this.transform.forward;
            lookRight = this.transform.right;

            Rigidbody.velocity = lookFoward.normalized * Status.spd;
            animator.SetBool("isWalkingForward", true);
            animator.SetBool("isIdle", false);
        }
    }

    public override int ModifyHealthPoint(int amount)
    {
        if (this.healthPoint == 0) return -1;
        if (!isDamageable) return 0;
        isDamageable = false;
        isActionable = false;
        StopAllCoroutines();
        StartCoroutine(CallMethodWaitForSeconds(1.5f, () => { isDamageable = true; }));
        return base.ModifyHealthPoint(amount);
    }

    public override void Die()
    {
        base.Die();
        isActionable = false;
        isActing = true;
        Debug.Log(true);
    }

    protected override void OnCollisionEnter(Collision other)
    {
    }

    protected override void OnCollisionStay(Collision other)
    {
    }

    public override void AnimEvt(string cmd)
    {
        isActing = false;
        switch (cmd)
        {
            case "AttackEnd":
                isAttacking = false;
                //this.transform.LookAt(playerInstance.transform);
                break;
            case "Damaged":
                isAttacking = false;
                isActionable = true;
                break;
            case "Kick":
                playerInstance.ModifyHealthPoint(Status.attackPoint * -1);
                StartCoroutine(playerInstance.HitBack(playerInstance.lookFoward * -1, 0.1f, 3f));
                break;
            case "KickEnd":
                isActionable = true;
                break;
        }
    }

    private IEnumerator Casting()
    {
        int i = 0;
        foreach (var _object in magicInstances)
        {
            var instance = Instantiate(_object);
            instance.gameObject.tag = "Enemy";
            instance.gameObject.GetComponent<MagicComponent>().caster = this;
            var offset =  i != 0 ? new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-20f, 20f)) : Vector3.zero;
            instance.transform.position += playerInstance.transform.position + offset;
            i += 1;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator CallMethodWaitForSeconds(float duration, Action act)
    {
        yield return new WaitForSeconds(duration);
        act();
    }
}
