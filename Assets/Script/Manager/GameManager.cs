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
    public bool isPause;

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
        if (scene.name == "Stage")
        {
            StartCoroutine(StageInitialize());
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
        PhotonNetwork.Instantiate("Player", new Vector3(9, 3 * MyPlayerNum + 1, 0), Quaternion.identity);
    }

    void SetGameArea(int[] tiles)
    {
        for (int i = 1; i < tileManager.areaCount; i++)
        {
            GameObject temp = tileManager.areas.bossAreas[tiles[i]];
            temp = PhotonNetwork.Instantiate(temp.name, temp.transform.position + new Vector3(30 * i, 0, 0), Quaternion.identity);
        }

        photonView.RPC("SetTileItem", RpcTarget.AllViaServer, 9, 3, "T_01", 0);
        photonView.RPC("SetTileItem", RpcTarget.AllViaServer, 9, 5, "T_02", 0);
    }

    [PunRPC]
    public void SetTileItem(int x, int y, string _to, int count)
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
}