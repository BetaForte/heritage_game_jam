using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource[] bgm;
    public AudioSource[] sfx;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(int index)
    {
        sfx[index].Play();
    }

    public void StopSFX(int index)
    {
        sfx[index].Stop();
    }

}
