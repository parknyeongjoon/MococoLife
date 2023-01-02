using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
/// <summary>
/// PhotonNetwork.AutomaticallySyncScene = true로 하면 방장의 씬을 동기화 할 수 있음 - 기능 추가
/// make room name or code do not duplicate
/// </summary>
public class NetworkManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "0";

    [SerializeField] Text connectStateText;

    void Start()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.IsConnected)
        {
            connectStateText.text = "Connect";
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
        connectStateText.text = "Connect";
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectStateText.text = "Fail to connect server...";
        Debug.Log("OnDisconnected");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedLobby()
    {
        connectStateText.text = "Join Lobby";
    }

    #region RoomManager
    [Header("RoomManager")]
    [SerializeField] GameObject roomPanel;
    [SerializeField] GameObject createPanel, joinPanel, roomGrid, roomPrefab;

    [SerializeField] TMP_InputField userName, roomName, joinRoomName;
    [SerializeField] Toggle isSecretRoom;

    Dictionary<string, GameObject> roomDic = new Dictionary<string, GameObject>();

    public void SetRoomPanel(bool isOpen)
    {
        if (PhotonNetwork.InLobby)
        {
            roomPanel.SetActive(isOpen);
        }
    }

    public void SetCreatePanel(bool isOpen)
    {
        createPanel.SetActive(isOpen);
    }

    public void SetJoinPanel(bool isOpen)
    {
        joinPanel.SetActive(isOpen);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject temp = null;

        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList)//room is delete
            {
                if (roomDic.TryGetValue(room.Name, out temp))
                {
                    roomDic.Remove(temp.name);
                    Destroy(temp);
                }
            }
            else//room is create or change
            {
                if (!roomDic.ContainsKey(room.Name))//create the room
                {
                    //instantiate room prefab
                    GameObject newRoom = Instantiate(roomPrefab, roomGrid.transform);
                    newRoom.GetComponent<RoomData>().RoomInfo = room;
                    roomDic.Add(room.Name, newRoom);
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

        if (string.IsNullOrEmpty(roomName.text)) { tempName = RandomRoomName(); }
        else
        {
            if (roomDic.ContainsKey(roomName.text))//if room name is duplicate
            {
                Debug.Log("room name duplicate");
                return;
            }
            tempName = roomName.text;
        }

        RoomOptions RO = new RoomOptions();
        RO.IsOpen = true;
        if (isSecretRoom.isOn) { RO.IsVisible = false; }//isSecret room => caanot random matching and see in room list
        else { RO.IsVisible = true; }
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

    public void JoinRoom()
    {
        if (roomDic.ContainsKey(joinRoomName.text))
        {
            PhotonNetwork.JoinRoom(joinRoomName.text);
        }
        else
        {
            Debug.Log("없는 방임");
        }
    }

    public override void OnJoinedRoom()
    {
        //setting nickname
        if (string.IsNullOrEmpty(userName.text))
        {
            string tempName = "NoName_" + Random.Range(0, 100).ToString();
            PhotonNetwork.NickName = tempName;
        }
        else
        {
            PhotonNetwork.NickName = userName.text;
        }

        PhotonNetwork.LoadLevel("Loading");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string temp = RandomRoomName();
        PhotonNetwork.CreateRoom(temp, new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 4 }); ;
    }

    string RandomRoomName()
    {
        string temp;

        do
        {
            temp = "Room_" + Random.Range(0, 1000);
        }
        while (roomDic.ContainsKey(temp));

        return temp;
    }

    #endregion
}
