using System;
using UnityEngine;
//오브젝트 풀링 테스트용 더미

namespace Component
{
    public class DummyComponent : MonoBehaviour
    {
        private void OnEnable()
        {
            this.transform.position = Vector3.zero;
        }

        private void Update()
        {
            if(this.transform.position.y < -10f)
                this.gameObject.SetActive(false);
        }
    }
}