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
    public int sceneNum; // �� Scene���� ��ȣ�� �˷��ֱ� ���� ����
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


    #region ���̵�
    public void Title_SceneMove() // ���ӿ��� ȭ�鿡�� Quit �������� ����Ǵ� �Լ�
    {
        Time.timeScale = 1;
        sceneNum = 0;
        StartCoroutine(Scene_Move());
    }
    public void Tutorial_SceneMove() // Ʃ�丮��(������ ��)
    {
        Debug.Log("Ʃ�丮�� �̵�");
        Time.timeScale = 1;
        sceneNum = 1;
        StartCoroutine(Scene_Move());
    }
    public void Stage1_SceneMove() // 1Stage(������ ��)
    {
        Time.timeScale = 1;
        sceneNum = 2;
        StartCoroutine(Scene_Move());

    }
    public void Stage2_SceneMove() // 2STage(������ ��)
    {
        Time.timeScale = 1;
        sceneNum = 3;
        StartCoroutine(Scene_Move());
    }

    public void Stage3_SceneMove() // 3Stage(�α�ȫ ��)
    {
        Time.timeScale = 1;
        sceneNum = 4;
        StartCoroutine(Scene_Move());
    }

    public void Stage4_SceneMove() // 4Stage(�ӽ¹� ��)
    {
        Time.timeScale = 1;
        sceneNum = 5;
        StartCoroutine(Scene_Move());
    }

    public void Stage5_SceneMove() // 5Stage(�迵�� ��)
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
                LoadingScene.LoadScene("��ȫ�� ���̸�");
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
    public void OnClick_QuitBtn() // �������� ��ư
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
        Debug.Log("����");
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

    #region ���ӿ���
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
