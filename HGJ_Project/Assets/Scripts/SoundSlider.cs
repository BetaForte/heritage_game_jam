using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    // Start is called before the first frame update

    public Slider slider;
    Text text;
    
    void Start()
    {
        text = GetComponent<Text>();
        UpdateSliderNumber();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSliderNumber()
    {
        int percentage = (int)(slider.value / slider.maxValue * 100.0f);
        text.text = percentage.ToString() + "%";
    }
}
