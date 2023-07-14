using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class LoadingScene : MonoBehaviour
{
    static string nextScene;
    public Image progressBar;
    //  public GameObject[] Loading_Bg;
    public GameObject[] Loading_Text;
    public GameObject[] Loading_Bg;
    int BG_Num;
    int Text_Num;
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");

    }
    void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadSceneProcess());
    }
    IEnumerator LoadSceneProcess()
    {
        switch (nextScene)
        {
            case "Tutorial":
                Scene_bg_text(0, true);
                break;
            case "Stage1":
                Scene_bg_text(1, true);
                break;
            case "Stage2":
                Scene_bg_text(2, true);
                break;
            case "Stage3":
                Scene_bg_text(3, true);
                break;
            case "Stage4":
                Scene_bg_text(4, true);
                break;
            case "Stage5":
                Scene_bg_text(5, true);
                break;
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.deltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
        switch (nextScene)
        {
            case "Tutorial":
                Scene_bg_text(0, false);
                break;
            case "Stage2":
                Scene_bg_text(1, false);
                break;
            case "Stage3":
                Scene_bg_text(2, false);
                break;
            case "Stage4":
                Scene_bg_text(3, false);
                break;
            case "Stage5":
                Scene_bg_text(4, false);
                break;
        }
    }
    void Scene_bg_text(int Snum, bool onoff)
    {
        Loading_Bg[Snum].SetActive(onoff);
        Loading_Text[Snum].SetActive(onoff);
    }
}
