using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetPosition : MonoBehaviour
{
    public Vector3 resetPoint;
    public Transform target;
    private Rigidbody rb;
    private int reset;
    private string sceneName;

    private void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        reset = PlayerPrefs.GetInt(sceneName);
        //Debug.Log(reset);
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (target.position.y < -20f || Input.GetKeyDown(KeyCode.O) || reset == 1)
        {
            ResetPos();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.SetInt(sceneName, 1);
            SceneManager.LoadScene(sceneName);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            int lastSceneIndex = PlayerPrefs.GetInt("LastScene");
            PlayerPrefs.SetInt(SceneManager.GetSceneByBuildIndex(lastSceneIndex + 1).name, 1);
            SceneManager.LoadScene(lastSceneIndex + 1);
        }
    }

    private void ResetPos()
    {
        //Debug.Log("reset");
        PlayerPrefs.SetInt(sceneName, 0);
        reset = 0;
        if (rb)
        {
            rb.velocity = Vector3.zero;
        }
        target.position = resetPoint;
    }
}
