using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public GameObject LoginUIPanel;
    public InputField playerNameInput;

    [Header("Lobby Panel")]
    public GameObject LobbyPanel;

    [Header("Connecting Panel")]
    public GameObject ConnectingPanel;

    private void Start()
    {
        LoginUIPanel.SetActive(true);
        ConnectingPanel.SetActive(false);
        LobbyPanel.SetActive(false);

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void CreateAndJoinRoom()
    {
        string randomRoomName = "Room" + Random.Range(0, 10000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    #region UI Callback Methods

    public void OnLoginButtonClicked()
    {
        string playerName = playerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;

            LoginUIPanel.SetActive(false);
            ConnectingPanel.SetActive(true);

            if(!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                Debug.Log("Player name is invalid!");
            }
        }
    }

    public void OnQuickGameButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    #endregion


    #region Photon Callbacks

    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon");
        ConnectingPanel.SetActive(false);
        LobbyPanel.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("ArenaGreyBox");
    }


    #endregion

}
