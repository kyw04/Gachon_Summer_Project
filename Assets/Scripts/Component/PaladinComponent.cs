using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PaladinComponent : BattleableComponentBase
{
    public bool isActing;
    public bool isActionable;
    public float coolDown;

    [SerializeField]
    private PlayerComponent playerInstance;

    delegate void Act();
    // Start is called before the first frame update
    void Start()
    {
        Status = new BattleableVOBase(){spd = 2};
        playerInstance = GameObject.FindWithTag("Player").GetComponent<PlayerComponent>();
        healthPoint = Status.maxHealthPoint;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Think();
        Move();
    }

    private void Think()
    {
        if (isActing || !isActionable) return;

        isActionable = false;
        Act action = Cast;

        var distance = Vector3.Distance(this.transform.position, playerInstance.transform.position);
        if (distance <= 1.5f)
            action = () =>
            {
                isActing = true;
                animator.SetTrigger("Kick");
            };
        else if (distance <= 3f)
            action = Attack;
        
        StartCoroutine(CallMethodWaitForSeconds(coolDown, () => { isActionable = true;}));
        action();
    }
    
    public void Cast()
    {
        isActing = true;
        animator.SetTrigger("Cast");

        var Instance = Instantiate(Resources.Load("Magic/Metor/Magic") as GameObject);
        Instance.GetComponent<MagicComponent>().caster = this;
        Instance.gameObject.tag = "Enemy";
        Instance.transform.position += playerInstance.transform.position;
    }

    public override void Attack()
    {
        base.Attack();
        isActing = true;
        animator.SetBool("isWalkingForward", false);
    }

    public override void Move()
    {
        if (isActing) return;
        this.transform.LookAt(playerInstance.transform);

        var distance = Vector3.Distance(this.transform.position, playerInstance.transform.position);
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
        isActionable = false;
        return base.ModifyHealthPoint(amount);
    }

    public override void Die()
    {
        base.Die();
        isActionable = false;
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
                StartCoroutine(playerInstance.HitBack(playerInstance.lookFoward * -1, 0.1f, 4f));
                break;
            case "KickEnd":
                isActionable = true;
                break;
        }
    }
    
    public IEnumerator CallMethodWaitForSeconds(float duration, Action act)
    {
        yield return new WaitForSeconds(duration);
        act();
    }
}
