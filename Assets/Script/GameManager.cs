using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Sword effect 만들기
 * 캐릭터 interactive 다시 손보기
 * BlackSmith 재료 넣는 기능 넣기
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
    public int myPlayerNum;///get으로 나중에 바꿔주기(손상되었을 때 일어날 오류가 클 거 같음), 로비에서 사람이 나가질 거 대비하기
    public PlayerInfo[] players = new PlayerInfo[4];
    public Dictionary<string, ItemData> itemDic = new Dictionary<string, ItemData>();

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
            Debug.Log("Stage 씬 시작");
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
        GameObject myPlayer = PhotonNetwork.Instantiate("Player", new Vector3(4, 3 * myPlayerNum, 0), Quaternion.identity);
        players[myPlayerNum] = myPlayer.GetComponent<PlayerInfo>();
    }

    [PunRPC]
    void SetGameArea(int[] tiles)//photonview를 달아가면서까지 PhotonNetwork.Instantiate를 사용할 필요가 있을까?
    {
        for (int i = 0; i < tileManager.areaCount; i++)
        {
            GameObject temp = Instantiate(tileManager.areas.bossAreas[tiles[i]]);
            temp.transform.position += new Vector3(30 * i, 0, 0);
        }
        SetTileItem(2, 3, "T_01", 1);

        SetTileItem(2, 5, "T_02", 1);

        tileManager.rightBoundary.transform.position = new Vector3(30 * tileManager.areaCount - 15, 0, 0);
    }

    //tileBase 쓰는 법 찾기
    [PunRPC]
    public void SetTileItem(int x, int y, string _to, int count)///_to를 null로 받는 건 너무 위험한가
    {
        if (_to == "T_00" || _to == null)
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
