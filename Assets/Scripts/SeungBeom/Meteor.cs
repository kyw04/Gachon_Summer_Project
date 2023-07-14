using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    GameObject Target;
    bool canattacked;
    // Start is called before the first frame update
    void OnEnable()
    {
        Target = FindObjectOfType<PlayerComponent>().gameObject;
        StartCoroutine(collision());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {

        if(other.gameObject == Target)
        {
            canattacked = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == Target)
        {
            canattacked = false;
        }
    }
    IEnumerator collision()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            if (canattacked) Target.SendMessage("Damaged", 2.5f);
        }
    }
}
