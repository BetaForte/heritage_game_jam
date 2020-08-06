using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider bgmSlider;


    private void Update()
    {
        foreach(AudioSource aSource in SoundManager.instance.bgm)
        {
            aSource.volume = bgmSlider.value;
        }

        foreach (AudioSource aSource in SoundManager.instance.sfx)
        {
            aSource.volume = sfxSlider.value;
        }


    }


    public void ToggleFullscreen(bool fullscreen)
    {
        if(fullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

}
