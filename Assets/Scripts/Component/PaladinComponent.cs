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

        if (Vector3.Distance(this.transform.position, playerInstance.transform.position) <= 3f)
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

        if (Vector3.Distance(this.transform.position, playerInstance.transform.position) <= 3f)
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

    protected override void OnCollisionEnter(Collision other)
    {
    }

    protected override void OnCollisionStay(Collision other)
    {
    }

    public override void AnimEvt(string cmd)
    {
        isActing = !isActing;
        switch (cmd)
        {
            case "AttackEnd":
                isAttacking = false;
                this.transform.LookAt(playerInstance.transform);
                break;
            case "Damaged":
                isAttacking = false;;
                break;
        }
    }
    
    public IEnumerator CallMethodWaitForSeconds(float duration, Action act)
    {
        yield return new WaitForSeconds(duration);
        act();
    }
}
