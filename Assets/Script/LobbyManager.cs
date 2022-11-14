using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "0";

    [SerializeField] Text connectStateText;

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            connectStateText.text = "Online";
        }
        else
        {
            connectStateText.text = "Connecting to server...";
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        connectStateText.text = "Online";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectStateText.text = "Fail to connect server...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void QuickMatching()
    {
        if (PhotonNetwork.IsConnected)
        {
            connectStateText.text = "Quick matching...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectStateText.text = "Fail to connect server...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Loading");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("Room_" + Random.Range(0, 1000), new RoomOptions { MaxPlayers = 4 });
    }
}
