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
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastScene") + 1);
    }
}
