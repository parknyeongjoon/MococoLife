using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class RoomData : MonoBehaviour
{
    [SerializeField] Button roomBtn;
    [SerializeField] TMP_Text roomName, playerCntText;
    [SerializeField] Image bossImg;

    RoomInfo roomInfo;

    public RoomInfo RoomInfo
    {
        get { return roomInfo; }

        set
        {
            roomInfo = value;
            roomName.text = roomInfo.Name;
            playerCntText.text = roomInfo.PlayerCount.ToString() + "/" + roomInfo.MaxPlayers.ToString();

            roomBtn.onClick.AddListener(EnterRoom);
        }
    }

    void EnterRoom()
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
    }
}
