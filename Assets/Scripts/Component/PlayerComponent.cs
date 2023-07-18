//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rope))]
public sealed class PlayerComponent : BattleableComponentBase, IControllable
{
    Stage2_Boss boss;
    public Boss_Line bossline;
    #region  Variable

    public bool isDodging = false;
    public bool isWiring = false;
    public bool isJumping = false;
    public bool isControllable = true;
    public bool isJumpable = true;
    [SerializeField] AudioClip[] clip;
    [SerializeField] private GameObject hook;
    private Hook _hookController;
    private int _attackStatus = 0;

    private WeaponComponent _weapon;

    #endregion
    delegate void Act();

    [Header("HP관련")]
    private float maxHp = 100f; // 플레이어 최대 체력
    private float curHp = 100f; // 플레이어 현재 체력
    [SerializeField] public Text hp_T;
    [SerializeField] public Slider hp_Bar;

    private Coroutine rollInstance;

    //Awake는 base에 사용 중이므로 기본 설정은 Start를 통해 해주세요
    private void Awake()
    {
        base.Awake();
        _hookController = GetComponent<Hook>();
        Instantiate(hook);
    }

    private void Start()
    {
        // BtnManager.instance.sceneNum = 3;
        SetUpPlayer();
        _weapon = GetComponentInChildren<WeaponComponent>();
        hp_Bar.maxValue = Status.maxHealthPoint;
        hp_Bar.value = healthPoint.Value;
        hp_T.text = curHp + "/" + maxHp;

        this.UpdateAsObservable()
            .Where(_ => Input.GetButtonDown("Fire1"))
            .Subscribe(param =>
            { _hookController.HookControl(true); });

        this.UpdateAsObservable()
            .Where(_ => Input.GetButtonDown("Fire2"))
            .Subscribe(param => { _hookController.HookControl(false); });
    }

    private void FixedUpdate()
    {
        Command();
        Status.position = this.transform.position;
        dataController.UseUpdate(Status.id, Status.position);
    }
    #region 기능적인 메소드

    void Damaged(float damage)
    {
        if (curHp > 0)
        {
            curHp -= damage;
            hp_Bar.value = healthPoint.Value;
            hp_T.text = curHp.ToString() + "/" + maxHp.ToString();
        }
        else if (curHp <= 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Gameover");
        }
    }

    public override int ModifyHealthPoint(int amount)
    {
        isControllable = false;
        var result = base.ModifyHealthPoint(amount);
        StopAllCoroutines();
        hp_Bar.value = healthPoint.Value;
        hp_T.text = $" {healthPoint} / {Status.maxHealthPoint}";
        return result;
    }

    public override void Die()
    {
        base.Die();
        //플레이어의 사망시 발생할 상황을 구현
        isControllable = false;
    }

    private void SetUpPlayer()
    {
        dataController = new PlayerDataController("/Battleable.db");
        Status = dataController.getDatum();

        animator.enabled = false;

        var playerModel = Resources.Load<GameObject>($"PlayerModels/{Status.modelName}/Model");

        GameObject modelInstance = null;

        try
        {
            modelInstance = Instantiate(playerModel);
        }
        catch
        {
            Debug.Log($"Do Not Found {Status.modelName} Prefab.\n Use Dummy Object");
            playerModel = Resources.Load<GameObject>("PlayerModels/Dummy/Model");
            modelInstance = Instantiate(playerModel);
        }
        finally
        {

            foreach (var child in this.transform.GetComponentsInChildren<Transform>())
            {
                if (child.transform == this.transform || child.gameObject.name == this.gameObject.name)
                    continue;
                child.gameObject.SetActive(false);
            }

            animator.avatar = modelInstance.GetComponent<Animator>().avatar;

            modelInstance.transform.position = Vector3.zero;
            modelInstance.transform.parent = this.transform;
        }

        GC.SuppressFinalize(playerModel);

        this.transform.position = Status.position;
        healthPoint.Value = Status.maxHealthPoint;
        StaminaPoint = Status.maxStaminaPoint;

        animator.Rebind();
        animator.enabled = true;

        Destroy(this.transform.GetChild(0).gameObject);
    }

    #endregion

    #region 플레이어 조작과 관련한 메소드

    public override void Attack()
    {
        SoundManager.instance.Player_Sound(clip[0]);
        _weapon.gameObject.SetActive(true);
        base.Attack();
        animator.SetInteger("AttackType", _attackStatus++ % 2);
        //if (boss.away <= 5)
        //{
        //    boss.SendMessage("Damaged", 20f);
        //    SoundManager.instance.Player_Sound(clip[1]);
        //}

    }

    public void Command()
    {
        //기본적으로 MOVE함수를 실행시키며 특정한 INPUT이 있으면 그에 맞는 메소드를 실행
        if (healthPoint.Value == 0) return;

        Act action = Move;


        //조작키들은 임의로 정해진 키로 작동하므로 나중에 정해지면 수정 바람 (2023. 06. 29)
        if (Input.GetButton("Attack"))
        {
            if (isControllable) action = Attack;
            isControllable = false;
        }
        else if (Input.GetButton("Dodge"))
        {
            action = () =>
            {
                if (isDodging || !isControllable) return;
                else isDodging = !isDodging;
                isControllable = false;

                _weapon.gameObject.SetActive(false);

                //this.transform.forward = lookFoward * (Input.GetAxisRaw("Vertical") == -1 ? -1 : 1);
                animator.SetTrigger("Rolling");
            };
        }
        else if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
        {
            //action = _hookController.HookControl;
        }
        else if (Input.GetButton("Jump"))
        {
            action = () =>
            {
                if (healthPoint.Value <= 0 || !isJumpable || isJumping) return;
                isJumpable = false;
                isControllable = false;
                isJumping = true;
                _weapon.gameObject.SetActive(false);
                //점프 구현
                animator.SetTrigger("Jump");
            };
        }

        action();
    }
    public override void Move()
    {
        int snum = BtnManager.instance.sceneNum;

        if (isDodging) return;

        try
        {
            if ((snum == 3 && bossline.isPlayerMove) || snum != 3)
            {
                if (!isControllable) return;
                _weapon.gameObject.SetActive(false);
                var dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                //에니메이터에 있는 bool 타입의 파라미터들을 한 번에 false로
                var boolParamName =
                    from param in animator.parameters
                    where param.type == AnimatorControllerParameterType.Bool
                    select param.name;
                foreach (var paramName in boolParamName)
                    animator.SetBool(paramName, false);


                //플레이어가 이동 중 일 경우
                if (dir.magnitude != 0)
                {
                    this.transform.forward = lookFoward;
                    var moveDir = lookFoward * dir.y + lookRight * dir.x;
                    if (dir.x == 0)
                    {
                        animator.SetBool(dir.y > 0 ? "isWalkingForward" : "isWalkingBackward", true);
                    }
                    else
                    {
                        if (dir.y == 0)
                        {

                            animator.SetBool(dir.x < 0 ? "isWalkingLeft" : "isWalkingRight", true);
                        }
                        else
                        {
                            //오른쪽 앞 & 뒤
                            animator.SetBool(dir.y > 0 ? "isWalkingForward" : "isWalkingBackward", true);
                            animator.SetBool(dir.x < 0 ? "isWalkingLeft" : "isWalkingRight", true);
                            moveDir /= Mathf.Sqrt(2);
                        }
                    }

                    transform.position += moveDir * Time.deltaTime * Status.spd;
                }
                else
                {
                    animator.SetBool("isIdle", true);
                    _weapon.gameObject.SetActive(true);
                }

            }
        }
        catch
        {
            //에러가 발생했을 때도 계속 해서 움직 일 수 있도록 예외처리 했습니다. 이 블록은 건들지 말아주세요 
            if (!isControllable) return;
            _weapon.gameObject.SetActive(false);

            Debug.Log(true);

            var dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            //에니메이터에 있는 bool 타입의 파라미터들을 한 번에 false로
            var boolParamName =
                from param in animator.parameters
                where param.type == AnimatorControllerParameterType.Bool
                select param.name;
            foreach (var paramName in boolParamName)
                animator.SetBool(paramName, false);


            //플레이어가 이동 중 일 경우
            if (dir.magnitude != 0)
            {
                this.transform.forward = lookFoward;
                var moveDir = lookFoward * dir.y + lookRight * dir.x;
                if (dir.x == 0)
                {
                    animator.SetBool(dir.y > 0 ? "isWalkingForward" : "isWalkingBackward", true);
                }
                else
                {
                    if (dir.y == 0)
                    {

                        animator.SetBool(dir.x < 0 ? "isWalkingLeft" : "isWalkingRight", true);
                    }
                    else
                    {
                        //오른쪽 앞 & 뒤
                        animator.SetBool(dir.y > 0 ? "isWalkingForward" : "isWalkingBackward", true);
                        animator.SetBool(dir.x < 0 ? "isWalkingLeft" : "isWalkingRight", true);
                        moveDir /= Mathf.Sqrt(2);
                    }
                }

                transform.position += moveDir * Time.deltaTime * Status.spd;
            }
            else
            {
                animator.SetBool("isIdle", true);
                _weapon.gameObject.SetActive(true);
            }
        }
    }
    #endregion

    #region Collision Func

    protected override void OnCollisionEnter(Collision other)
    {

    }

    protected override void OnCollisionStay(Collision other)
    {
        isJumpable = true;
    }

    private void OnCollisionExit(Collision other)
    {
        isJumpable = false;
    }

    #endregion

    public override void AnimEvt(string cmd)
    {
        isControllable = true;
        isJumpable = true;
        _weapon.gameObject.SetActive(true);
        switch (cmd)
        {
            case "AttackEnd":
                isAttacking = false;
                break;
            case "Roll":
                var status = Input.GetAxisRaw("Vertical");

                if (rollInstance is not null)
                {
                    StopCoroutine(rollInstance);
                    Rigidbody.velocity = Vector3.zero;
                    rollInstance = null;
                }

                rollInstance = StartCoroutine(Roll(status));
                break;
            case "RollEnd":
                isDodging = false;
                isJumping = false;
                break;
            case "Jump":
                _weapon.gameObject.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(Jump());
                break;
            case "JumpEnd":
                isJumping = false;
                break;
            case "Damaged":
                isAttacking = false;
                isJumping = false;
                isDodging = false;
                Rigidbody.useGravity = true;
                rollInstance = null;
                break;
        }

    }

    #region IEnumerator

    IEnumerator Roll(float status)
    {
        var dir = lookFoward;
        var coefficient = (Mathf.Abs(status) > 0.5f ? status : 1);
        this.transform.LookAt(dir.normalized * coefficient + this.transform.position);
        this.Rigidbody.velocity = this.transform.forward.normalized * 8f;
        while (isDodging)
        {
            this.Rigidbody.velocity += this.transform.forward.normalized * 0.3f;
            yield return new WaitForSeconds(0.1f);
        }

        Rigidbody.velocity = Vector3.zero;
        rollInstance = null;
        yield break;
    }
    IEnumerator Jump()
    {
        Rigidbody.useGravity = false;
        var coefficient = 1f;
        var degree = 0.2f;
        while (isJumping)
        {
            this.Rigidbody.velocity += Vector3.up * coefficient * 1.5f;
            coefficient -= degree;
            degree *= 0.86f;
            yield return new WaitForSeconds(0.01f);
        }
        Rigidbody.useGravity = true;
    }

    public IEnumerator HitBack(Vector3 dir, float duration, float power)
    {
        var time = 0f;
        while (true)
        {
            this.transform.position += dir.normalized * 0.05f * power;
            time += 0.02f;
            if (time >= duration) yield break;
            yield return new WaitForSeconds(0.01f);
        }
    }

    #endregion

}