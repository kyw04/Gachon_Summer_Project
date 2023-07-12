using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;

    [SerializeField] AudioSource _boss_EffectSource;
    [SerializeField] AudioSource _player_EffectSource;
    public AudioSource Bgm;
    public AudioClip[] Bgm_clips; // 0: ³· 1: ¹ã


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

    public void BGM_Sound(int clipNum)
    {
        Bgm.clip = Bgm_clips[clipNum];
        Bgm.Play();
    }

    public void Player_Sound(AudioClip clip)
    {
        _player_EffectSource.PlayOneShot(clip);
    }
}
