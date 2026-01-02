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

    AudioSource audio;
    AudioClip P3MagicCircleS;
    AudioClip P3EX;
    AudioClip P3Charge;

    SphereCollider sphere;
    // Start is called before the first frame update
    private void Awake()
    {
        Target = FindObjectOfType<PlayerComponent>().gameObject;
        audio = GetComponent<AudioSource>();
        sphere = GetComponent<SphereCollider>();
        P3MagicCircleS = Resources.Load<AudioClip>("SeungBeom/Sound/P3MC-1");
        P3EX = Resources.Load<AudioClip>("SeungBeom/Sound/P3EX");
        P3Charge = Resources.Load<AudioClip>("SeungBeom/Sound/ex1");
    }
    void OnEnable()
    {
        audio.clip = P3MagicCircleS;
        audio.Play();
        sphere.enabled = false;
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
        audio.PlayOneShot(P3Charge);
        Explosion.SetActive(true);
        LightOn = true;
        yield return new WaitForSeconds(4.99f);
        sphere.enabled = true;
        audio.clip = P3EX;
        audio.Play();
        yield return new WaitForSeconds(0.1f);    //Æø¹ßÇÔ
        sphere.enabled = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Target)
        {   
            Target.SendMessage("Damaged", 150);
        }
    }
}
