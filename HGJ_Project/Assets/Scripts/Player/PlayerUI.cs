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

    [Header("Nitro Gauge")]
    public GameObject nitroGauge;
    public Image nitroSlider;

    private void Start()
    {
        gauge.SetActive(false);
        nitroGauge.SetActive(false);
    }

    void Update()
    {
        PlayerCharging();


        if (player.fired)
        {
            nitroGauge.SetActive(true);
            nitroSlider.fillAmount = player.chargeValue / player.chargeTime;
        }
        else
        {
            nitroGauge.SetActive(false);
        }
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
