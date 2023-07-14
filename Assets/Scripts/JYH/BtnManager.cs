using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    [SerializeField] GameObject[] onoff_Btn;
    [SerializeField] GameObject[] onoff_sound_icon;
    [SerializeField] GameObject settingPopup;
    static public BtnManager instance;
    public int sceneNum; // °¢ SceneµéÀÇ ¹øÈ£¸¦ ¾Ë·ÁÁÖ±â À§ÇÑ º¯¼ö
    void Awake()
    {
        Debug.Log(sceneNum);

        if (instance == null)
            instance = this;
        else
            DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && sceneNum == 3)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            settingPopup.SetActive(true);
        }
    }


    #region ¾ÀÀÌµ¿
    public void Title_SceneMove() // °ÔÀÓ¿À¹ö È­¸é¿¡¼­ Quit ´­·¶À»¶§ ½ÇÇàµÇ´Â ÇÔ¼ö
    {
        Time.timeScale = 1;
        sceneNum = 0;
        StartCoroutine(Scene_Move());
    }
    public void Tutorial_SceneMove() // Æ©Åä¸®¾ó(À±Áö¼º ¾À)
    {
        Debug.Log("Æ©Åä¸®¾ó ÀÌµ¿");
        Time.timeScale = 1;
        sceneNum = 1;
        StartCoroutine(Scene_Move());
    }
    public void Stage1_SceneMove() // 1Stage(±èÅÂÀÎ ¾À)
    {
        Time.timeScale = 1;
        sceneNum = 2;
        StartCoroutine(Scene_Move());

    }
    public void Stage2_SceneMove() // 2STage(ÀüÀ¯ÇÑ ¾À)
    {
        Time.timeScale = 1;
        sceneNum = 3;
        StartCoroutine(Scene_Move());
    }

    public void Stage3_SceneMove() // 3Stage(¹Î±âÈ« ¾À)
    {
        Time.timeScale = 1;
        sceneNum = 4;
        StartCoroutine(Scene_Move());
    }

    public void Stage4_SceneMove() // 4Stage(ÀÓ½Â¹ü ¾À)
    {
        Time.timeScale = 1;
        sceneNum = 5;
        StartCoroutine(Scene_Move());
    }

    public void Stage5_SceneMove() // 5Stage(±è¿µ¿õ ¾À)
    {
        Time.timeScale = 1;
        sceneNum = 6;
        StartCoroutine(Scene_Move());
    }

    IEnumerator Scene_Move()
    {
        yield return new WaitForSeconds(0.5f);
        switch (sceneNum)
        {
            case 0:
                LoadingScene.LoadScene("Title");
                break;
            case 1:
                LoadingScene.LoadScene("Tutorial");
                break;
            case 2:
                LoadingScene.LoadScene("Stage1");
                break;
            case 3:
                LoadingScene.LoadScene("Stage2");
                break;
            case 4:
                LoadingScene.LoadScene("±âÈ«ÀÌ ¾ÀÀÌ¸§");
                break;
            case 5:
                LoadingScene.LoadScene("Stage4");
                break;
            case 6:
                LoadingScene.LoadScene("Stage5");
                break;
        }
    }
    #endregion

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
        onoff_Btn[0].SetActive(true);
        onoff_Btn[1].SetActive(false);
        onoff_sound_icon[0].SetActive(true);
        onoff_sound_icon[1].SetActive(false);
        SoundOnOff(true);
    }
    public void Off_Sound()
    {
        onoff_Btn[0].SetActive(false);
        onoff_Btn[1].SetActive(true);
        onoff_sound_icon[0].SetActive(false);
        onoff_sound_icon[1].SetActive(true);
        SoundOnOff(false);

    }

    void SoundOnOff(bool isSound)
    {
        Debug.Log("µé¾î¿È");
        SoundManager.instance._boss_EffectSource.mute = isSound;
        SoundManager.instance._player_EffectSource.mute = isSound;
        SoundManager.instance.Bgm.mute = isSound;
    }

    public void Continue_Btn()
    {
        Time.timeScale = 1;
        settingPopup.SetActive(false);
    }
    #endregion

    #region °ÔÀÓ¿À¹ö
    public void GameOver_Retry()
    {
        switch (sceneNum)
        {
            case 2:
                LoadingScene.LoadScene("Stage1");
                break;
            case 3:
                LoadingScene.LoadScene("Stage2");
                break;
            case 4:
                LoadingScene.LoadScene("Stage3");
                break;
            case 5:
                LoadingScene.LoadScene("Stage4");
                break;
            case 6:
                LoadingScene.LoadScene("Stage5");
                break;
        }
    }
    #endregion
}
