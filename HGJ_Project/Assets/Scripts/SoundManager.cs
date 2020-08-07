using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource[] bgm;
    public AudioSource[] sfx;

    float countUp = 0;
    [HideInInspector] public bool playLobbyMusic = false;

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

    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            bgm[0].Stop();
            if(countUp < 1)
            {
                countUp += Time.deltaTime / 2;
            }
            if(!playLobbyMusic)
            {
                bgm[1].Play();
                playLobbyMusic = true;
            }
            bgm[1].volume = countUp;

        }
    }

    public void PlaySFX(int index)
    {
        sfx[index].Play();
    }

    public void PlayOneShotSFX(int index)
    {
        sfx[index].PlayOneShot(sfx[index].clip);
    }

    public void StopSFX(int index)
    {
        sfx[index].Stop();
    }



}
