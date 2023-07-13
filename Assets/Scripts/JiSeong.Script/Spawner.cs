using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectPrefab; // 소환할 오브젝트 프리팹
    public Transform spawnPoint; // 오브젝트를 소환할 위치

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
