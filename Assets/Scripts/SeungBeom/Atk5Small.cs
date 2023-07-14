using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk5Small : MonoBehaviour
{
    GameObject Meteor;
    GameObject explo;
    GameObject player;
    GameObject MagicCircle;
    bool Grow;
    bool Shrink;

    SphereCollider sphere;
    // Start is called before the first frame update
    private void Awake()
    {
        sphere = GetComponent<SphereCollider>();
        Meteor = transform.GetChild(0).gameObject;
        explo = transform.GetChild(1).gameObject;
        MagicCircle = transform.GetChild(2).gameObject;
        Meteor.SetActive(false);
        explo.SetActive(false);
        MagicCircle.SetActive(false);
        player = FindObjectOfType<PlayerComponent>().gameObject;
    }
    void OnEnable()
    {
        Meteor.SetActive(false);
        explo.SetActive(false);
        MagicCircle.SetActive(false);
        StartCoroutine(Ex());
        sphere.enabled = false;
    }
    private void Update()
    {
        if (Grow)
        {
            MagicCircle.transform.localScale += new Vector3(42 * Time.deltaTime, 42 * Time.deltaTime, 42 * Time.deltaTime);
        }
        if (Shrink)
        {
            MagicCircle.transform.localScale -= new Vector3(21 * Time.deltaTime, 21 * Time.deltaTime, 21 * Time.deltaTime);
        }
    }

    IEnumerator Ex()
    {
        MagicCircle.SetActive(true);
        Grow = true;
        yield return new WaitForSeconds(0.5f);
        Grow = false;
        yield return new WaitForSeconds(3.5f);
        Meteor.SetActive(true);
        Shrink = true;
        yield return new WaitForSeconds(0.6f);
        sphere.enabled = true;
        yield return new WaitForSeconds(0.1f);
        explo.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        sphere.enabled = false;
        yield return new WaitForSeconds(0.2f);
        Shrink = false;
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            player.SendMessage("Damaged", 40);
        }
    }
}
