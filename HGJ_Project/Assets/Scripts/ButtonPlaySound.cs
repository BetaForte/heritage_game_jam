using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPlaySound : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler
{
    public int highlightSound;
    public int clickSound;



    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.instance.PlaySFX(highlightSound);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SoundManager.instance.PlaySFX(clickSound);
    }




}
