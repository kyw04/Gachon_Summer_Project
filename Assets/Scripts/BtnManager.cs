using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    private int sceneNum; // 각 Scene들의 번호를 알려주기 위한 변수
    Animator anim;

    public void OnClick_StartBtn() // Title에서 Start 눌렀을때 실행되는 함수
    {
        sceneNum = 1;
        StartCoroutine(Scene_Move());
    }

    public void OnClick_QuitBtn()
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
            case 1:
                LoadingScene.LoadScene("JYH");
                break;
        }
    }
}
