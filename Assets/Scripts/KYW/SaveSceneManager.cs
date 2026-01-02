using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSceneManager : MonoBehaviour
{
    private void Awake()
    {
        //Debug.Log(SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetString("LastSceneName", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);
        Debug.Log(PlayerPrefs.GetInt("LastScene"));
    }
}
