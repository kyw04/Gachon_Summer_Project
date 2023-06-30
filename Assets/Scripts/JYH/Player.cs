using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController cc;
    public float gravityScale;
    public float jumpPower;
    float y;
    public float Speed;
    public float CurSpeed;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        CurSpeed = Speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        y -= gravityScale * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded)
        {
            y = jumpPower;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
            CurSpeed = Speed + 3;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            CurSpeed = Speed;

        Vector3 dir = new Vector3(h, y, v);
        dir = transform.TransformDirection(dir);
        dir.y = y;
        cc.Move(dir * CurSpeed * Time.deltaTime);
    }
}
