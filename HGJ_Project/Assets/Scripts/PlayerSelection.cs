using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Player
{
    Player1,
    Player2,
    Player3,
    Player4
}

public class PlayerSelection : MonoBehaviour
{

    public Player player;

    public GameObject[] vehicles;
    public GameObject currentVehicle;
    public int index;


    private void Update()
    {
        for(int i = 0; i < vehicles.Length; i++)
        {
            if(i != index)
            {
                vehicles[i].SetActive(false);
            }
            else
            {
                vehicles[index].SetActive(true);
            }
        }

        currentVehicle = vehicles[index];

        switch (player)
        {
            case Player.Player1:
                ArenaSettings.instance.player1Vehicle = currentVehicle.name;
                break;
            case Player.Player2:
                ArenaSettings.instance.player2Vehicle = currentVehicle.name;
                break;
            case Player.Player3:
                ArenaSettings.instance.player3Vehicle = currentVehicle.name;
                break;
            case Player.Player4:
                ArenaSettings.instance.player4Vehicle = currentVehicle.name;
                break;
        }
    }


    public void OnNextButtonClicked()
    {
        if(index >= vehicles.Length - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
    }

    public void OnBackButtonClicked()
    {
        if(index <= vehicles.Length)
        {
            index = vehicles.Length - 1;
        }
        else
        {
            index--;
        }
    }


}
