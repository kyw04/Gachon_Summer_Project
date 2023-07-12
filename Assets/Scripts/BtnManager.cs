using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    private int sceneNum; // �� Scene���� ��ȣ�� �˷��ֱ� ���� ����
    Animator anim;

    

    public void OnClick_NextScene() // ����1 Ŭ���� ȭ�鿡�� Start �������� ����Ǵ� �Լ�
    {
        sceneNum = 6;
        StartCoroutine(Scene_Move());

    }

    public void OnClick_GameOver_Restart() // ���ӿ��� ȭ�鿡�� Restart �������� ����Ǵ� �Լ�
    {
        sceneNum = 2;
        StartCoroutine(Scene_Move());

    }

    public void OnClick_GameOver_Quit() // ���ӿ��� ȭ�鿡�� Quit �������� ����Ǵ� �Լ�
    {
        sceneNum = 0;
        StartCoroutine(Scene_Move());

    }
    public void OnClick_StartBtn() // Title���� Start �������� ����Ǵ� �Լ�
    {
        sceneNum = 1;
        StartCoroutine(Scene_Move());
    }

    public void OnClick_QuitBtn() // �������� ��ư
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif 
        Application.Quit();
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
                LoadingScene.LoadScene("JYH");
                break;
            case 2:
                LoadingScene.LoadScene("kti");
                break;
            case 6:
                LoadingScene.LoadScene("SB(Boss)");
                break;
        }
    }
}
