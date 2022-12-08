using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Sword effect �����
 * ĳ���� interactive �ٽ� �պ���(�ذ�)
 * BlackSmith ��� �ִ� ��� �ֱ�(�ذ�)
 * overlapPoint ���� �ֵ� physics2D.isTouchingLayers �ᵵ �� ��?
 * players �޾ƿ� ��� �����ϱ�
 * ĳ���� flipX ����ȭ�ϱ�
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

    public Boss boss;

    public float gameSpeed;
    public int difficulty;

    [Header("Data")]
    private int myPlayerNum;///�κ񿡼� ����� ������ �� ����ϱ�
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
            photonView.RPC("SetGameArea", RpcTarget.AllBufferedViaServer, tiles);
        }
        //�÷��̾� ����
        PhotonNetwork.Instantiate("Player", new Vector3(4, 3 * MyPlayerNum + 1, 0), Quaternion.identity);
    }

    [PunRPC] void SetGameArea(int[] tiles)//photonview�� �޾ư��鼭���� PhotonNetwork.Instantiate�� ����� �ʿ䰡 ������?
    {
        for (int i = 0; i < tileManager.areaCount; i++)
        {
            GameObject temp = Instantiate(tileManager.areas.bossAreas[tiles[i]]);
            temp.transform.position += new Vector3(30 * i, 0, 0);
        }
        SetTileItem(2, 3, "T_01", 0);

        SetTileItem(2, 5, "T_02", 0);

        tileManager.rightBoundary.transform.position += new Vector3(30 * tileManager.areaCount, 0, 0);
    }

    [PunRPC] public void SetTileItem(int x, int y, string _to, int count)
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
