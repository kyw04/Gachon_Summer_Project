using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    static string nextScene;
    public Image progressBar;
    //  public GameObject[] Loading_Bg;
    public GameObject[] Loading_Text;
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
        //BG_Num = Random.Range(0, 3);
        Text_Num     = 0; //Random.Range(0, 7);
        StartCoroutine(LoadSceneProcess());
    }
    IEnumerator LoadSceneProcess()
    {
        //  Loading_Bg[BG_Num].SetActive(true);
        Loading_Text[Text_Num].SetActive(true);

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
        //   Loading_Bg[BG_Num].SetActive(false);
        Loading_Text[Text_Num].SetActive(false);
    }
}
