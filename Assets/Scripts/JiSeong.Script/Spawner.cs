using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectPrefab; // ��ȯ�� ������Ʈ ������
    public Transform spawnPoint; // ������Ʈ�� ��ȯ�� ��ġ

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
