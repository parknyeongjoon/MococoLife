using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                return null;
            }
            return instance;
        }
    }

    PhotonView PV;

    [Header("Game")]
    public bool isPause;

    public Boss boss;

    public float gameSpeed;
    public int difficulty;
    public int tileCount;

    [Header("Data")]
    public testPlayerMove[] players = new testPlayerMove[4];
    public Dictionary<string, ItemData> itemDic = new Dictionary<string, ItemData>();
    public AreaSO areas;

    //Ready
    public bool isGameReady;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        PV = photonView;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();//옮기기
        StartCoroutine(SetGame());
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)//Scene이 호출될 때 실행되는 함수
    {
        Debug.Log(scene.name);
        Debug.Log(mode);
    }

    IEnumerator SetGame()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        RPCSetGameArea();
    }

    #region Tile

    public void RPCSetGameArea()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int[] tiles = new int[tileCount];

            for (int i = 0; i < tileCount; i++)
            {
                tiles[i] = Random.Range(0, areas.bossAreas.Count);
            }

            PV.RPC("SetGameArea", RpcTarget.AllBufferedViaServer, tiles);
        }
    }

    [PunRPC]
    void SetGameArea(int[] tiles)//photonview를 달아가면서까지 PhotonNetwork.Instantiate를 사용할 필요가 있을까?
    {
        for (int i = 0; i < tileCount; i++)
        {
            Instantiate(areas.bossAreas[tiles[i]], new Vector3(30 * i, 0, 0), Quaternion.identity);
        }

        GameObject.Find("RightBoundary").transform.position = new Vector3(30 * tileCount - 15, 0, 0);
    }

    #endregion

    #region NetworkManage

    public override void OnConnectedToMaster()//최적의 룸으로 접속 혹은 생성
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 4;

        PhotonNetwork.JoinOrCreateRoom("TestRoom", roomOption, null);
    }

    public override void OnJoinedRoom()//방에 접속했을 때(Player Script 바꾸기)
    {
        Room room = PhotonNetwork.CurrentRoom;
        GameObject myPlayer = PhotonNetwork.Instantiate("Player", new Vector3(0, room.PlayerCount * 3 - 7.5f), Quaternion.identity);
        players[room.PlayerCount - 1] = myPlayer.GetComponent<testPlayerMove>();
    }

    public override void OnLeftRoom()//떠날 때 해당 인덱스의 플레이어 기록 지우기
    {

    }

    #endregion
}
