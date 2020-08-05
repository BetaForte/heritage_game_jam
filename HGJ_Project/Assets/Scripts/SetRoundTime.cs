using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetRoundTime : MonoBehaviour
{
    public float[] time;
    public int index;

    public Text currentTimeText;

    private void Update()
    {
        ArenaSettings.instance.roundTime = time[index];

        if(index == 0)
            currentTimeText.text = "30 SECONDS";
        if (index == 1)
            currentTimeText.text = "1 MINUTE";
        if (index == 2)
            currentTimeText.text = "2 MINUTE";
        if (index == 3)
            currentTimeText.text = "3 MINUTE";
        if (index == 4)
            currentTimeText.text = "5 MINUTE";
    }

    public void OnRightButtonClicked()
    {
        if (index >= time.Length - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
    }

    public void OnLeftButtonClicked()
    {
        if (index <= 0)
        {
            index = time.Length - 1;
        }
        else
        {
            index--;
        }
    }

}
