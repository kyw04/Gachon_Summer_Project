using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiSeong
{
    public class Player : MonoBehaviour
    {
        protected Rigidbody rb;
        protected float moveSpeed = 0.01f;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.UpArrow))
                transform.Translate(Vector3.forward * moveSpeed);
            if (Input.GetKey(KeyCode.DownArrow))
                transform.Translate(Vector3.back * moveSpeed);
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Translate(Vector3.left * moveSpeed);
            if (Input.GetKey(KeyCode.RightArrow))
                transform.Translate(Vector3.right * moveSpeed);
            if (Input.GetKey(KeyCode.U))
                transform.Translate(Vector3.up * moveSpeed);
            if (Input.GetKey(KeyCode.D))
                transform.Translate(Vector3.down * moveSpeed);
        }
    }

}