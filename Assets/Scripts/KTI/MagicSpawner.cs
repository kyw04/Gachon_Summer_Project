using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpawner : MonoBehaviour
{
    public GameObject rangeObject;
    BoxCollider rangeCollider;


    private void Awake()
    {
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
    }

    Vector3 Return_RandomPosition()
    {
        Vector3 originPosition = rangeObject.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;


        range_X = Random.Range(-21, 35);
        range_Z = Random.Range(-21, 35);
        Vector3 RandomPostion = new Vector3(range_X, 0, range_Z);

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
            for (int i = 0; i < 1; i++)
            {
                Instantiate(enemy, Return_RandomPosition(), Quaternion.identity);
            }


        }
    }
}


