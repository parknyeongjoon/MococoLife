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

    [Header("Game")]
    public bool isPause;

    public Boss boss;

    public float gameSpeed;
    public int difficulty;

    [Header("Data")]
    public int myPlayerNum;
    public PlayerMove[] players = new PlayerMove[4];
    public Dictionary<string, ItemData> itemDic = new Dictionary<string, ItemData>();

    //Ready
    public bool[] isGameReady = new bool[4];

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)//Scene�� ȣ��� �� ����Ǵ� �Լ�
    {
        if (scene.name == "Stage")
        {
            Debug.Log("Stage �� ����");
            StartCoroutine(StageInitialize());
        }
    }

    #region Tiles

    IEnumerator StageInitialize()
    {
        yield return new WaitUntil(() => TileManager.Instance != null);
        if (PhotonNetwork.IsMasterClient)
        {
            //GameArea ����
            int[] tiles = TileManager.Instance.AreaInitialize();
            photonView.RPC("SetGameArea", RpcTarget.AllBufferedViaServer, tiles);
        }
        //�÷��̾� ����
        PhotonNetwork.Instantiate("Player", new Vector3(7, 3 * myPlayerNum, 0), Quaternion.identity);
    }

    [PunRPC]
    void SetGameArea(int[] tiles)//photonview�� �޾ư��鼭���� PhotonNetwork.Instantiate�� ����� �ʿ䰡 ������?
    {
        TileManager tileManager = TileManager.Instance;

        for (int i = 0; i < tileManager.tileCount; i++)
        {
            GameObject temp = Instantiate(tileManager.areas.bossAreas[tiles[i]]);
            temp.transform.position += new Vector3(30 * i, 0, 0);
        }
        tileManager.rightBoundary.transform.position = new Vector3(30 * tileManager.tileCount - 15, 0, 0);
    }

    #endregion
}
