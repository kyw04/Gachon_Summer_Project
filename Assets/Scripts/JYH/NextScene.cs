using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void NextStage_Button()
    {
        int lastSceneIndex = PlayerPrefs.GetInt("LastScene");
        PlayerPrefs.SetInt(SceneManager.GetSceneByBuildIndex(lastSceneIndex + 1).name, 1);
        SceneManager.LoadScene(lastSceneIndex + 1);
    }
}
