using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;

    [SerializeField] AudioSource _bgmSource, _boss_EffectSource;

    private void Awake()
    {
        if (instance == null)
        {
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        else
            Destroy(gameObject);
    }


    public void Boss_PlaySound(AudioClip clip)
    {
        _boss_EffectSource.PlayOneShot(clip);
    }

    public void BGM_PlaySound(AudioClip clip)
    {
        _bgmSource.PlayOneShot(clip);
    }


}
