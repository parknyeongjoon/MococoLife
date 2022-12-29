using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
/// <summary>
/// PhotonNetwork.AutomaticallySyncScene = true로 하면 방장의 씬을 동기화 할 수 있음 - 기능 추가
/// </summary>
public class NetworkManager : MonoBehaviourPunCallbacks
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

    #region RoomManager
    [Header("RoomManager")]
    [SerializeField] GameObject roomPanel;
    [SerializeField] GameObject createPanel;

    [SerializeField] TMP_InputField userName;
    [SerializeField] TMP_InputField roomName;
    [SerializeField] TMP_InputField password;

    Dictionary<string, GameObject> roomDic = new Dictionary<string, GameObject>();

    public void SetRoomPanel(bool isOpen)
    {
        roomPanel.SetActive(isOpen);
    }

    public void SetCreatePanel(bool isOpen)
    {
        createPanel.SetActive(isOpen);
    }

    void UpdateRoomPanel(List<RoomInfo> roomInfos)
    {
        GameObject temp = null;

        foreach(RoomInfo room in roomInfos)
        {
            if(room.RemovedFromList)//room is delete
            {
                roomDic.TryGetValue(room.Name, out temp);
                Destroy(temp);
                roomDic.Remove(temp.name);
            }
            else//room is create or change
            {
                if(!roomDic.ContainsKey(room.Name))//create the room
                {
                    //instantiate room prefab
                }
                else
                {
                    //update the room
                }
            }
        }
    }

    public void CreateRoom()
    {
        string tempName;
        if(string.IsNullOrEmpty(roomName.text))
        {
            tempName = "Room_" + Random.Range(0, 1000).ToString();
        }
        else
        {
            tempName = roomName.name;
        }

        RoomOptions RO = new RoomOptions();
        RO.IsOpen = true;
        RO.IsVisible = true;
        RO.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(tempName, RO);
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
        //setting nickname
        if(string.IsNullOrEmpty(userName.text))
        {
            string tempName = "NoName_" + Random.Range(0, 100).ToString();
            PhotonNetwork.NickName = tempName;
        }
        else
        {
            PhotonNetwork.NickName = userName.text;
        }
        //index의 nickname text 변경해주기

        PhotonNetwork.LoadLevel("Loading");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("Room_" + Random.Range(0, 1000), new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 4 });;
    }

    #endregion
}
