using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// T는 풀링할 오브젝트
public abstract class ObjectPoolComponentBase : MonoBehaviour
{
    [SerializeField] protected GameObject prefab = null;
    [SerializeField] protected float frequency = 1f;
   
    //풀링할 오브젝트의 최대치를 가진 상수
    public const int MaxObjectCount = 20;
    
    public Queue<GameObject> poolObjectQueue;

    public ObjectPoolComponentBase() 
    {
        poolObjectQueue = new Queue<GameObject>();

        if (prefab == null) Debug.LogError("Cannot found Prefab");
        
        for (int i = 0; i < MaxObjectCount; i++)
        {
            poolObjectQueue.Enqueue(Instantiate(prefab));
        }
    }
    public ObjectPoolComponentBase(float frequency)
    {
        this.frequency = frequency;
        
    }

    public void StartPooling(Queue<GameObject> queue)
    {
        StartCoroutine(Pooling(queue));
    }

    IEnumerator Pooling(Queue<GameObject> queue)
    {
        while (true)
        {
            var instance =  queue.Dequeue();
            instance.SetActive(true);
            queue.Enqueue(instance);

            yield return new WaitForSeconds(frequency);
        }
    }
}