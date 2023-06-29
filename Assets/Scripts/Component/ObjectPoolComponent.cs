using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

// T는 풀링할 오브젝트
public class ObjectPoolComponent : MonoBehaviour
{
    //풀링할 오브젝트
    [SerializeField] private GameObject prefab = null;
    //오브젝트를 생성하는 주기
    [SerializeField] private float frequency = 1f;
    //풀링할 오브젝트의 최대치를 가진 변수
    [SerializeField] private int maxObjectCount = 20;
    
    public Queue<GameObject> poolObjectQueue;
    private bool _isPooling = false;

    private void Awake()
    {
        poolObjectQueue = new Queue<GameObject>();

        if (prefab == null) Debug.LogError("Cannot found Prefab");
        
        for (int i = 0; i < maxObjectCount; i++)
        {
            var item = Instantiate(prefab, this.transform, true);
            item.SetActive(false);
            poolObjectQueue.Enqueue(item);
        }
    }

    private void Start()
    {
        StartPooling(poolObjectQueue);
    }

    public void StartPooling(Queue<GameObject> queue)
    {
        StartCoroutine(Pooling(queue));
    }

    public void StopPooling()
    {
        _isPooling = !_isPooling;
    }

    IEnumerator Pooling(Queue<GameObject> queue)
    {
        _isPooling = true;
        while (_isPooling)
        {
            var instance =  queue.Dequeue();
            instance.SetActive(true);
            queue.Enqueue(instance);

            yield return new WaitForSeconds(frequency);
        }
    }
}