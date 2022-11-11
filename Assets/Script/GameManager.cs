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

    public PhotonView PV;

    [Header("Game")]
    public bool isPause;

    public Boss boss;

    public float gameSpeed;
    public int difficulty;

    [Header("Data")]
    public testPlayerMove[] players = new testPlayerMove[4];
    public Dictionary<string, ItemData> itemDic = new Dictionary<string, ItemData>();

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
        PhotonNetwork.ConnectUsingSettings();//�ű��
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)//Scene�� ȣ��� �� ����Ǵ� �Լ�
    {
        Debug.Log(scene.name);
        Debug.Log(mode);
    }

    #region Tiles

    [PunRPC]
    void SetGameArea(int[] tiles)//photonview�� �޾ư��鼭���� PhotonNetwork.Instantiate�� ����� �ʿ䰡 ������?
    {
        Debug.Log("1");
        TileManager tileManager = TileManager.Instance;
        Debug.Log("2");

        for (int i = 0; i < tileManager.tileCount; i++)
        {
            Instantiate(tileManager.areas.bossAreas[tiles[i]], new Vector3(30 * i, 0, 0), Quaternion.identity);
        }
        Debug.Log("3");
        tileManager.rightBoundary.transform.position = new Vector3(30 * tileManager.tileCount - 15, 0, 0);
    }

    #endregion

    #region NetworkManage

    public override void OnConnectedToMaster()//������ ������ ���� Ȥ�� ����
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 4;

        PhotonNetwork.JoinOrCreateRoom("TestRoom", roomOption, null);
    }

    public override void OnJoinedRoom()//�濡 �������� ��(Player Script �ٲٱ�)
    {
        Room room = PhotonNetwork.CurrentRoom;
        GameObject myPlayer = PhotonNetwork.Instantiate("Player", new Vector3(0, room.PlayerCount * 3 - 7.5f), Quaternion.identity);
        players[room.PlayerCount - 1] = myPlayer.GetComponent<testPlayerMove>();
    }

    public override void OnLeftRoom()//���� �� �ش� �ε����� �÷��̾� ��� �����
    {

    }

    #endregion
}
