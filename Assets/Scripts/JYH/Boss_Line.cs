using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Line : MonoBehaviour
{
    public Stage2_Boss boss;
    public GameObject boss_Torch;
    public GameObject boss_Wall;
    public bool isArrive_Boss;
    public bool endProduction;

    public AudioClip[] clip;

    [Header("보스 등장 연출 관련")]
    public GameObject boss_name_obj;
    public Text bossNameText;
    public float delayBetweenCharacters = 0.1f;
    string bossName;
    int currentIndex;
    public bool isPlayerMove;

    [Header("보스 등장 연출을 위한 카메라")]
    public GameObject player_Camera;
    public GameObject boss_Camera;

    private IEnumerator on;

    void Start()
    {
        isPlayerMove = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && on == null)
        {
            Time.timeScale = 0.8f;

            SoundManager.instance.Boss_PlaySound(clip[0]);
            on = PlayerCamera_OnOff();
            ShowBossNameEffect("Death Bringer");
            StartCoroutine(on);
            boss_Torch.SetActive(true);
            boss_Wall.SetActive(true);
            isArrive_Boss = true;
            Destroy(gameObject, 4f);
        }
    }

    #region   Text 
    void ShowBossNameEffect(string name)
    {
        bossName = name;
        currentIndex = 0;
        StartCoroutine(ShowBossName());
    }
    IEnumerator ShowBossName()
    {
        bossNameText.text = string.Empty;

        while (currentIndex < bossName.Length)
        {
            bossNameText.text += bossName[currentIndex];
            currentIndex++;
            yield return new WaitForSeconds(delayBetweenCharacters);
        }
    }
    #endregion

    IEnumerator PlayerCamera_OnOff()
    {
        Time.timeScale = 1;
        isPlayerMove = false;
        boss_Camera.SetActive(true);
        player_Camera.SetActive(false);
        yield return new WaitForSeconds(3f);
        isPlayerMove = true;
        boss_name_obj.SetActive(false);
        boss_Camera.SetActive(false);
        player_Camera.SetActive(true);
        boss.SendMessage("Health_Bar");
        endProduction = true;
    }
}