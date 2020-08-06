using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerMovement player;

    [Header("Charging Gauge")]
    public GameObject gauge;
    public Image slider;


    private void Start()
    {
        gauge.SetActive(false);
    }

    void Update()
    {
        PlayerCharging();

    }

    private void PlayerCharging()
    {
        if (player.charging)
        {
            gauge.SetActive(true);
            slider.fillAmount = player.chargeValue / player.chargeTime;
        }
        else
        {
            gauge.SetActive(false);
        }
    }
}
