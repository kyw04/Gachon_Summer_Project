using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    [SerializeField] GameObject[] onoff_t;
    [SerializeField] GameObject[] onoff_sound_icon;
    [SerializeField] GameObject settingPopup;
    static public BtnManager instance;
    public int sceneNum; // °¢ SceneµéÀÇ ¹øÈ£¸¦ ¾Ë·ÁÁÖ±â À§ÇÑ º¯¼ö
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            settingPopup.SetActive(true);
        }
    }
    public void Title_SceneMove() // °ÔÀÓ¿À¹ö È­¸é¿¡¼­ Quit ´­·¶À»¶§ ½ÇÇàµÇ´Â ÇÔ¼ö
    {
        sceneNum = 0;
        StartCoroutine(Scene_Move());

    }
    public void Tutorial_SceneMove() // Æ©Åä¸®¾ó(À±Áö¼º ¾À)
    {
        sceneNum = 1;
        StartCoroutine(Scene_Move());
    }
    public void Stage1_SceneMove() // 1Stage(±èÅÂÀÎ ¾À)
    {
        sceneNum = 2;
        StartCoroutine(Scene_Move());

    }
    public void Stage2_SceneMove() // 2STage(ÀüÀ¯ÇÑ ¾À)
    {
        sceneNum = 3;
        StartCoroutine(Scene_Move());
    }

    public void Stage3_SceneMove() // 3Stage(¹Î±âÈ« ¾À)
    {
        sceneNum = 4;
        StartCoroutine(Scene_Move());
    }

    public void Stage4_SceneMove() // 4Stage(ÀÓ½Â¹ü ¾À)
    {
        sceneNum = 5;
        StartCoroutine(Scene_Move());
    }

    public void Stage5_SceneMove() // 5Stage(±è¿µ¿õ ¾À)
    {
        sceneNum = 6;
        StartCoroutine(Scene_Move());
    }


    #region Title Scene 
    public void OnClick_QuitBtn() // °ÔÀÓÁ¾·á ¹öÆ°
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif 
        Application.Quit();
    }
    #endregion


    #region Setting
    public void On_Sound()
    {
        onoff_t[0].SetActive(true);
        onoff_t[1].SetActive(false);
        onoff_sound_icon[0].SetActive(true);
        onoff_sound_icon[1].SetActive(false);
    }
    public void Off_Sound()
    {
        onoff_t[0].SetActive(false);
        onoff_t[1].SetActive(true);
        onoff_sound_icon[1].SetActive(false);
        onoff_sound_icon[1].SetActive(true);
    }
    #endregion
    IEnumerator Scene_Move()
    {
        yield return new WaitForSeconds(0.5f);
        switch (sceneNum)
        {
            case 0:
                LoadingScene.LoadScene("Title");
                break;
            case 1:
                LoadingScene.LoadScene("Jiseong.Ex");
                break;
            case 2:
                LoadingScene.LoadScene("kti");
                break;
            case 3:
                LoadingScene.LoadScene("JYH");
                break;
            case 4:
                LoadingScene.LoadScene("±âÈ«ÀÌ ¾ÀÀÌ¸§");
                break;
            case 5:
                LoadingScene.LoadScene("SB(Boss)");
                break;
            case 6:
                LoadingScene.LoadScene("Dragon");
                break;

        }
    }
}
