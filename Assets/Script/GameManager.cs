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
    //public testPlayerMove[] players = new testPlayerMove[4];
    public Dictionary<string, ItemData> itemDic = new Dictionary<string, ItemData>();

    //Ready
    public bool[] isGameReady = new bool[4];

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        PV = photonView;
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        TileManager tileManager = TileManager.Instance;

        for (int i = 0; i < tileManager.tileCount; i++)
        {
            Instantiate(tileManager.areas.bossAreas[tiles[i]], new Vector3(30 * i, 0, 0), Quaternion.identity);
        }
        tileManager.rightBoundary.transform.position = new Vector3(30 * tileManager.tileCount - 15, 0, 0);
    }

    #endregion
}
