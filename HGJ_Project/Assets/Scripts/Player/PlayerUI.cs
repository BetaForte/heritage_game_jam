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

    [Header("Current Speed")]
    public GameObject speedMetre;
    public Text speedText;

    private void Start()
    {
        gauge.SetActive(false);
        speedMetre.SetActive(true);
    }

    void Update()
    {
        PlayerCharging();

        speedText.text = Mathf.Round(player.chargeValue) + " km/h";
    }

    private void PlayerCharging()
    {
        if (player.charging)
        {
            gauge.SetActive(true);
            slider.fillAmount = player.chargeValue / player.maxChargeValue;
        }
        else
        {
            gauge.SetActive(false);
        }
    }
}
