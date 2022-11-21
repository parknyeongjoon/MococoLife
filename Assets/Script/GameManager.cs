using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.Tilemaps;
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
    [Header("Manager")]
    TileManager tileManager;

    [Header("Game")]
    public bool isPause;

    public Boss boss;

    public float gameSpeed;
    public int difficulty;

    [Header("Data")]
    public int myPlayerNum;
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
        ChangeTile(2, 3, "T_01", 1);

        tileManager.rightBoundary.transform.position = new Vector3(30 * tileManager.areaCount - 15, 0, 0);
    }

    //tileBase 쓰는 법 찾기
    [PunRPC]
    public void ChangeTile(int x, int y, string _to, int count)///_to를 null로 받는 건 너무 위험한가
    {
        if (_to == "")
        {
            tileManager.tileInfos[x][y].tileSlot.itemData = null;
            tileManager.tileInfos[x][y].tileSlot.itemCount = 0;
            Vector3Int temp = tileManager.terrainTileMap.WorldToCell(new Vector3Int(x, y));//월드에서 타일 좌표로 변환
            tileManager.terrainTileMap.SetTile(temp, null);//지우기
        }
        else
        {
            tileManager.tileInfos[x][y].tileSlot.itemData = itemDic[_to];
            tileManager.tileInfos[x][y].tileSlot.itemCount = count;
            Vector3Int temp = tileManager.terrainTileMap.WorldToCell(new Vector3Int(x % 30, y));//월드에서 타일 좌표로 변환
            tileManager.terrainTileMap.SetTile(temp, itemDic[_to].tileImg);//특정 아이템 타일로 그려주기
        }

    }

    #endregion
}
