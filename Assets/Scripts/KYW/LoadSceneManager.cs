using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartButton()
    {
        int lastSceneIndex = PlayerPrefs.GetInt("LastScene");
        PlayerPrefs.SetInt(PlayerPrefs.GetString("LastSceneName"), 1);
        SceneManager.LoadScene(lastSceneIndex);
    }
}
