using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Production : MonoBehaviour
{
    public Text bossNameText;
    public float delayBetweenCharacters = 0.1f;

    private string bossName;
    private int currentIndex;

    public void ShowBossNameEffect(string name)
    {
        bossName = name;
        currentIndex = 0;
        StartCoroutine(ShowBossName());
    }
    private IEnumerator ShowBossName()
    {
        bossNameText.text = string.Empty;

        while (currentIndex < bossName.Length)
        {
            bossNameText.text += bossName[currentIndex];
            currentIndex++;
            yield return new WaitForSeconds(delayBetweenCharacters);
        }
    }
}
