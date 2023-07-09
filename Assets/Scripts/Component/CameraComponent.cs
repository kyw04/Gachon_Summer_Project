using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraComponent : MonoBehaviour
{
    public BattleableComponentBase followingTarget;
    //카메라 위치 조정할 때 쓰는 변수
    public Vector3 offset;
    
    private Animator _animator;
    private Transform _anchor;
    // Start is called before the first frame update
    void Start()
    {
        //마우스 위치 가운데로 고정
        //cursorLockMode.none으로 복구 가능
        Cursor.lockState = CursorLockMode.Locked;
        
        _animator = followingTarget.animator;
        _anchor = this.transform.parent;

        followingTarget.lookFoward = _anchor.transform.forward.normalized;
        followingTarget.lookFoward = _anchor.transform.right.normalized;

    }

    // Update is called once per frame
    void Update()
    {
        LookAt();

    }

    private void LookAt()
    {
        var mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        var camAngle = _anchor.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;

        x = x < 180f ? Mathf.Clamp(x, -1, 25) : Mathf.Clamp(x, 290, 361); 
        
        _anchor.rotation = Quaternion.Euler(
            x, camAngle.y + mouseDelta.x,
            camAngle.z
        );
        _anchor.transform.position = followingTarget.transform.position + offset;

        if (followingTarget is PlayerComponent)
        {
            
            followingTarget.lookFoward = new Vector3(
                _anchor.forward.x,
                0,
                _anchor.forward.z
            ).normalized;
        
            followingTarget.lookRight = new Vector3(
                _anchor.right.x,
                0,
                _anchor.right.z
            ).normalized;
        }
    }
}
