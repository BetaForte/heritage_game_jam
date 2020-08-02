using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ArenaGameManager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;
    public Transform spawnPosition;

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            GameObject player = PhotonNetwork.Instantiate(playerPrefabs[0].name, spawnPosition.position, Quaternion.identity);

            Transform playerBody = player.transform.Find("Player");

            playerBody.transform.position = spawnPosition.position;

        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " has joined");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " has joined");
    }

}
