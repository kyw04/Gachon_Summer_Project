using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk3Ex : MonoBehaviour
{
    bool Grow;
    bool Shrink;
    bool canattack;
    GameObject Target;

    public GameObject Circle;
    public GameObject Explosion;
    bool LightOn;
    Light ligh;
    // Start is called before the first frame update
    private void Awake()
    {
        Target = FindObjectOfType<PlayerComponent>().gameObject;
    }
    void OnEnable()
    {
        Circle = transform.GetChild(0).gameObject;
        Explosion = transform.GetChild(1).gameObject;
        ligh = GetComponent<Light>();

        Explosion.SetActive(false);
        Circle.SetActive(true);

        StartCoroutine(Growing());
    }

    // Update is called once per frame
    void Update()
    {
        if (Grow)
        {
            Circle.transform.localScale += new Vector3(25 * Time.deltaTime, 25 * Time.deltaTime, 25 * Time.deltaTime);
        }
        if (Shrink)
        {
            Circle.transform.localScale -= new Vector3(25 * Time.deltaTime, 25 * Time.deltaTime, 25 * Time.deltaTime);
        }
        if(LightOn)
        {
            ligh.range += 100 * Time.deltaTime;
            ligh.intensity += 2f * Time.deltaTime;
        }
    }
    IEnumerator Growing()
    {
        ligh.enabled = true;
        Grow = true;
        yield return new WaitForSeconds(2f);
        Grow = false;

        yield return new WaitForSeconds(3f);
        Explosion.SetActive(true);
        LightOn = true;
        yield return new WaitForSeconds(4.9f);
        if (canattack) Target.SendMessage("Damaged", 70f);
        yield return new WaitForSeconds(0.1f);    //폭발함
        
        LightOn = false;
        ligh.range = 0;
        ligh.intensity = 1;
        ligh.enabled = false;
        Shrink = true;
        yield return new WaitForSeconds(2f);
        Shrink = false;
        Circle.transform.localScale = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == Target)
        {
            canattack = true;
            Debug.Log("맞는다");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject == Target)
        {
            canattack = false;
            Debug.Log("나왔다");
        }
    }
}
