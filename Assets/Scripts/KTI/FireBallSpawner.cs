using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpawner : MonoBehaviour
{
    // 위에서 언급한 Plane의 자식인 RespawnRange 오브젝트
    public GameObject rangeObject;
    BoxCollider rangeCollider;


    [Header("파이어볼 개수")]
    public int FireballCount;

    private void Awake()
    {
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
    }

    Vector3 Return_RandomPosition()
    {
        Vector3 originPosition = rangeObject.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider.bounds.size.x;
        


        range_X = Random.Range(-25, 35);
      

        Vector3 RandomPostion = new Vector3(range_X, 0, -25);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }

    public GameObject enemy;
    private void Start()
    {
        StartCoroutine(RandomRespawn_Coroutine());
    }

    IEnumerator RandomRespawn_Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            // 생성 위치 부분에 위에서 만든 함수 Return_RandomPosition() 함수 대입
            //GameObject instantCapsul = Instantiate(enemy, Return_RandomPosition(), Quaternion.identity);
            for (int i = 0; i < FireballCount; i++)
            {
                Instantiate(enemy, Return_RandomPosition(), Quaternion.identity);
            }
        }
    }
}
