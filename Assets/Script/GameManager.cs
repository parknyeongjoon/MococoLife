using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Sword effect 만들기
 * 캐릭터 interactive 다시 손보기(해결)
 * BlackSmith 재료 넣는 기능 넣기(해결)
 * overlapPoint 쓰는 애들 physics2D.isTouchingLayers 써도 될 듯?
 * players 받아올 방법 생각하기
 * 캐릭터 flipX 동기화하기
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
    private int myPlayerNum;///로비에서 사람이 나가질 거 대비하기
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)//Scene이 호출될 때 실행되는 함수
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
            //GameArea 제작
            int[] tiles = tileManager.AreaInitialize();
            photonView.RPC("SetGameArea", RpcTarget.AllBufferedViaServer, tiles);
        }
        //플레이어 생성
        PhotonNetwork.Instantiate("Player", new Vector3(4, 3 * MyPlayerNum + 1, 0), Quaternion.identity);
    }

    [PunRPC] void SetGameArea(int[] tiles)//photonview를 달아가면서까지 PhotonNetwork.Instantiate를 사용할 필요가 있을까?
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
        if (_to == "T_00" || itemDic[_to].Item_Type == Item_Type.BattleItem)//플레이어가 맨손이었거나 배템을 들고있었다면
        {
            tileManager.tileInfos[x][y].tileSlot.itemData = null;
            tileManager.tileInfos[x][y].tileSlot.itemCount = 0;
            Vector3Int temp = tileManager.terrainTileMap.WorldToCell(new Vector3Int(x, y));//월드에서 타일 좌표로 변환
            tileManager.terrainTileMap.SetTile(temp, null);//지우기
        }
        else if (itemDic.ContainsKey(_to))
        {
            tileManager.tileInfos[x][y].tileSlot.itemData = itemDic[_to];
            tileManager.tileInfos[x][y].tileSlot.itemCount = count;
            Vector3Int temp = tileManager.terrainTileMap.WorldToCell(new Vector3Int(x, y));//월드에서 타일 좌표로 변환
            tileManager.terrainTileMap.SetTile(temp, itemDic[_to].tileImg);//특정 아이템 타일로 그려주기
        }
        else
        {
            Debug.Log("itemDic에 없는 아이템");
        }

    }

    #endregion
}
