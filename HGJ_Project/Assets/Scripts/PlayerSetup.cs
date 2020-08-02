using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPun
{
    public GameObject playerBody;
    public GameObject canvas;
    public GameObject cinemachineCamera;

    private void Start()
    {
        if(playerBody.GetComponent<PhotonView>().IsMine)
        {
            playerBody.GetComponent<PlayerMovement>().enabled = true;
            canvas.SetActive(true);
            cinemachineCamera.SetActive(true);
        }
        else
        {
            playerBody.GetComponent<PlayerMovement>().enabled = false;
            canvas.SetActive(false);
            cinemachineCamera.SetActive(false);
        }


    }


}
