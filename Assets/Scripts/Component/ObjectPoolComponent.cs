using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

// T는 풀링할 오브젝트
public class ObjectPoolComponent : MonoBehaviour
{
    //풀링할 오브젝트
    [SerializeField] private GameObject prefab = null;
    //오브젝트를 생성하는 주기
    //[SerializeField] private float frequency = 1f;
    //풀링할 오브젝트의 최대치를 가진 변수
    [SerializeField] private int maxObjectCount = 20;

    private int index;
    
    public List<GameObject> poolObjectLlist;
    //private bool _isPooling = false;

    private void Awake()
    {
        poolObjectLlist = new List<GameObject>();
        index = -1;
        if (prefab == null) Debug.LogError("Cannot found Prefab");
        
        for (int i = 0; i < maxObjectCount; i++)
        {
            var item = Instantiate(prefab, this.transform, true);
            item.SetActive(false);
            poolObjectLlist.Add(item);
        }
    }

    private void Start()
    {
        //StartPooling(poolObjectLlist);
    }

    public GameObject GetItem(Vector3 pos)
    {
        if (poolObjectLlist.Count == 0)
            return null;

        index = (index + 1) % maxObjectCount;

        var item = poolObjectLlist[index];

        if (item.activeSelf == false)
        {
            item.SetActive(false);
        }

        item.transform.position = pos;

        item.SetActive(true);

        return item;
    }

    public GameObject GetItem(Transform parent)
    {
        if (poolObjectLlist.Count == 0)
            return null;

        index = (index + 1) % maxObjectCount;

        var item = poolObjectLlist[index];

        if (item.activeSelf == false)
        {
            item.SetActive(false);
        }
        
        item.transform.parent = parent;
        item.transform.position = Vector3.zero;

        item.SetActive(true);

        return item;
    }

    public bool FreeItem(GameObject item) // 
    {
        int freeItemIndex = poolObjectLlist.IndexOf(item);

        if (freeItemIndex == -1)
            return false;

        GameObject freeItem = poolObjectLlist[freeItemIndex];
        poolObjectLlist.RemoveAt(freeItemIndex);

        freeItem.SetActive(false);
        freeItem.transform.rotation = Quaternion.identity;
        
        if (freeItem.GetComponent<Rigidbody>())
        {
            freeItem.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        index = (index + 1) % maxObjectCount;
        poolObjectLlist.Insert(index, item);

        return true;
    }

    public bool FreeItem(GameObject item, float seconds) // 
    {
        StartCoroutine(LaterFreeItem(item, seconds));
        return true;
    }

    private IEnumerator LaterFreeItem(GameObject item, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        FreeItem(item);
    }

    //public void StartPooling(Queue<GameObject> queue)
    //{
    //    StartCoroutine(Pooling(queue));
    //}

    //public void StopPooling()
    //{
    //    _isPooling = !_isPooling;
    //}

    //IEnumerator Pooling(Queue<GameObject> queue)
    //{
    //    _isPooling = true;
    //    while (_isPooling)
    //    {
    //        var instance =  queue.Dequeue();
    //        instance.SetActive(true);
    //        queue.Enqueue(instance);

    //        yield return new WaitForSeconds(frequency);
    //    }
    //}
}