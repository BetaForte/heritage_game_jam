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

    [Header("Pause Menu")]
    public GameObject pauseMenu;

    private void Start()
    {
        gauge.SetActive(false);
        speedMetre.SetActive(true);
        speedText.text = "0 km/h";
    }

    void Update()
    {
        PlayerCharging();

        if(player.fired)
        {
            speedText.text = Mathf.Round(player.chargeValue) + " km/h";
        }
    }

    public void OnPauseButtonClicked()
    {
        pauseMenu.SetActive(true);
    }

    public void OnResumeButtonClicked()
    {
        pauseMenu.SetActive(false);
    }

    public void OnQuitButtonClicked()
    {

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
