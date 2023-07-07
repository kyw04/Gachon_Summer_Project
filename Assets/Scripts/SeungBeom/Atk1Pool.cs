using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk1Pool : MonoBehaviour
{

    public GameObject prefab;
    public int maxCount = 250;

    public Queue<GameObject> queue = new Queue<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < maxCount; i++)
        {
            GameObject Poolobjcet = Instantiate(prefab);
            queue.Enqueue(Poolobjcet);
            Poolobjcet.SetActive(false);
        }
    }

    public void ENQUEUE(GameObject poolobjcet)
    {
        queue.Enqueue(poolobjcet);
        poolobjcet.SetActive(false);
    }
    public GameObject DEQUEUE()
    {
        GameObject poolobject = queue.Dequeue();
        poolobject.SetActive(true);

        queue.Enqueue(poolobject);
        return poolobject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
