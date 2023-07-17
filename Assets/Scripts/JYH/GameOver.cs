using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    int resultnum;
    void Start()
    {
        BtnManager btn = FindObjectOfType<BtnManager>();
        int resultnum = btn.sceneNum;
        Debug.Log(resultnum);
    }
}