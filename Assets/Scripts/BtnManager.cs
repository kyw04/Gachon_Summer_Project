using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    private int sceneNum; // �� Scene���� ��ȣ�� �˷��ֱ� ���� ����
    Animator anim;

    public void OnClick_StartBtn() // Title���� Start �������� ����Ǵ� �Լ�
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
