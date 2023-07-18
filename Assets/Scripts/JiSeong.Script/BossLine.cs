using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class BossLine : MonoBehaviour
{
    public ZBoss boss;
    public bool isArrive_Boss;
    public bool endProduction;

    public AudioClip[] clip;

    [Header("보스 등장 연출 관련")]
    public GameObject boss_name_obj;
    public Text bossNameText;
    public float delayBetweenCharacters = 0.1f;
    public Slider boss_hp;
    public GameObject boss_hp_obj;
    string bossName;
    int currentIndex;

    [Header("보스 등장 연출을 위한 카메라")]
    public GameObject player_Camera;
    public GameObject boss_Camera;
    public GameObject prisonObject;

    private IEnumerator on;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && on == null)
        {
            boss_hp_obj.SetActive(true);
            Time.timeScale = 0.8f;
            SoundManager.instance.Boss_PlaySound(clip[0]);
            on = PlayerCamera_OnOff();
            Destroy(prisonObject);
            ShowBossNameEffect("Abyssal Administrator");
            StartCoroutine(on);
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
        boss_Camera.SetActive(true);
        player_Camera.SetActive(false);
        yield return new WaitForSeconds(3f);
        boss_name_obj.SetActive(false);
        boss_Camera.SetActive(false);
        player_Camera.SetActive(true);
        endProduction = true;
    }
}