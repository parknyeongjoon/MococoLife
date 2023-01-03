using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Sword effect �����(�ذ�)
 * ĳ���� interactive �ٽ� �պ���(�ذ�)
 * BlackSmith ��� �ִ� ��� �ֱ�(�ذ�) - ���ü� �� ���̱�
 * players �޾ƿ� ��� �����ϱ�(�ذ�)
 * ĳ���� flipX ����ȭ�ϱ�(�ذ�)
 * PNum �κ񿡼� ����� ������ �� ����ϱ�
 * ����� ���� ���� ǥ���ϱ�
 * Option �����
 * ���� ���� Ŭ���� �����
 * ���� ��Ī�̶� �� �����(��й�ȣ Ȥ�� �ʴ�?) ��� �����
 * �÷��̾� ��Ų �����(Ȥ�� �������� ���� �����ϰ� �����)
 * ��� �÷��̾ �ε��� ������ ���� �ʱ�ȭ�� �ϰ� ���ÿ� �����ϱ�
 */
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

    [Header("Manager")]
    TileManager tileManager;

    [Header("Game")]
    public bool isPause = false;

    public BossInfo boss;

    public float gameSpeed;
    public int difficulty;

    [Header("Data")]
    private int myPlayerNum;
    public PlayerInfo[] players = new PlayerInfo[4];
    public Dictionary<string, ItemData> itemDic = new Dictionary<string, ItemData>();

    public int MyPlayerNum { get => myPlayerNum; }

    public void SetMyPlayerNum(int index) { myPlayerNum = index; }

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
        if(scene.name == "Loading")
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            isPause = false;
            Time.timeScale = 1;
            Pooler.Instance.ClearPools();
        }
        else if (scene.name == "Stage")
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            StartCoroutine(StageInitialize());
        }
    }

    [PunRPC] void GameClear()
    {
        if(isPause == false)
        {
            Debug.Log("Game Clear");
            isPause = true;
            UIManager.Instance.SetClearPanel(true);
        }
    }

    [PunRPC] void GameOver()
    {
        if(isPause == false)
        {
            Debug.Log("GameOver");
            isPause = true;
            UIManager.Instance.SetOverPanel(true);
        }
    }

    #region Tiles

    IEnumerator StageInitialize()
    {
        yield return new WaitUntil(() => TileManager.Instance != null);
        tileManager = TileManager.Instance;

        if (PhotonNetwork.IsMasterClient)
        {
            //GameArea ����
            int[] tiles = tileManager.AreaInitialize();
            SetGameArea(tiles);
        }
        //�÷��̾� ����
        PhotonNetwork.Instantiate("Player", new Vector3(9, 2 * MyPlayerNum + 2, 0), Quaternion.identity);
    }

    void SetGameArea(int[] tiles)
    {
        for (int i = 1; i < tileManager.areaCount; i++)
        {
            GameObject temp = tileManager.areas.bossAreas[tiles[i]];
            temp = PhotonNetwork.Instantiate(temp.name, temp.transform.position + new Vector3(39 * i, 0, 0), Quaternion.identity);
        }

        photonView.RPC("SetTileItem", RpcTarget.All, 11, 6, "T_01", 0);
        photonView.RPC("SetTileItem", RpcTarget.All, 11, 3, "T_02", 0);
    }

    [PunRPC]
    public void SetTileItem(int x, int y, string _to, int count)//rpc���� �����ϴ� �Լ��� ���� �����
    {
        if (_to == "T_00" || itemDic[_to].Item_Type == Item_Type.BattleItem)//�÷��̾ �Ǽ��̾��ų� ������ ����־��ٸ�
        {
            tileManager.tileInfos[x][y].tileSlot.itemData = null;
            tileManager.tileInfos[x][y].tileSlot.itemCount = 0;
            Vector3Int temp = tileManager.terrainTileMap.WorldToCell(new Vector3Int(x, y));//���忡�� Ÿ�� ��ǥ�� ��ȯ
            tileManager.terrainTileMap.SetTile(temp, null);//�����
        }
        else if (itemDic.ContainsKey(_to))
        {
            tileManager.tileInfos[x][y].tileSlot.itemData = itemDic[_to];
            tileManager.tileInfos[x][y].tileSlot.itemCount = count;
            Vector3Int temp = tileManager.terrainTileMap.WorldToCell(new Vector3Int(x, y));//���忡�� Ÿ�� ��ǥ�� ��ȯ
            tileManager.terrainTileMap.SetTile(temp, itemDic[_to].tileImg);//Ư�� ������ Ÿ�Ϸ� �׷��ֱ�
        }
        else
        {
            Debug.Log("itemDic�� ���� ������");
        }

    }

    #endregion

    [PunRPC]
    public void Ping(int type, Vector3 pos)
    {
        Instantiate(Resources.Load<GameObject>("Ping/" + type.ToString()), new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
    }
}