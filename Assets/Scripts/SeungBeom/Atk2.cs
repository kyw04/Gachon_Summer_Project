using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk2 : MonoBehaviour
{
    public bool Grow;
    public bool Shrink;
    public GameObject Meteor;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Growing");
    }

    // Update is called once per frame
    void Update()
    {
        //-------------- 메테오 낙하 전 마법진 생성 -------------------
        if (Grow)
        {
            transform.localScale += new Vector3(2*70 * Time.deltaTime, 2*70 * Time.deltaTime, 2*70 * Time.deltaTime);
        }
        if (Shrink)
        {
            transform.localScale -= new Vector3(2*70 * Time.deltaTime, 2*70 * Time.deltaTime, 2*70 * Time.deltaTime);
        }
    }
    //-------------- 메테오 낙하 전 마법진 생성 -------------------
    IEnumerator Growing()
    {

        Grow = true;
        yield return new WaitForSeconds(0.5f);
        Grow = false;

        yield return new WaitForSeconds(2.5f);  //3초 후에 메테오 생성.
        Instantiate(Meteor, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.4f);
        Shrink = true;
        //Instantiate(Meteor,transform.position,Quaternion.identity);
        
        yield return new WaitForSeconds(0.6f);
        gameObject.SetActive(false);
    }

    

}
