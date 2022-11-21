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
        tileManager = TileManager.Instance;
        if (PhotonNetwork.IsMasterClient)
        {
            //GameArea ����
            int[] tiles = tileManager.AreaInitialize();
            photonView.RPC("SetGameArea", RpcTarget.AllBufferedViaServer, tiles);
        }
        //�÷��̾� ����
        GameObject myPlayer = PhotonNetwork.Instantiate("Player", new Vector3(4, 3 * myPlayerNum, 0), Quaternion.identity);
        players[myPlayerNum] = myPlayer.GetComponent<PlayerInfo>();
    }

    [PunRPC]
    void SetGameArea(int[] tiles)//photonview�� �޾ư��鼭���� PhotonNetwork.Instantiate�� ����� �ʿ䰡 ������?
    {
        for (int i = 0; i < tileManager.areaCount; i++)
        {
            GameObject temp = Instantiate(tileManager.areas.bossAreas[tiles[i]]);
            temp.transform.position += new Vector3(30 * i, 0, 0);
        }
        ChangeTile(2, 3, "T_01", 1);

        tileManager.rightBoundary.transform.position = new Vector3(30 * tileManager.areaCount - 15, 0, 0);
    }

    //tileBase ���� �� ã��
    [PunRPC]
    public void ChangeTile(int x, int y, string _to, int count)///_to�� null�� �޴� �� �ʹ� �����Ѱ�
    {
        if (_to == "")
        {
            tileManager.tileInfos[x][y].tileSlot.itemData = null;
            tileManager.tileInfos[x][y].tileSlot.itemCount = 0;
            Vector3Int temp = tileManager.terrainTileMap.WorldToCell(new Vector3Int(x, y));//���忡�� Ÿ�� ��ǥ�� ��ȯ
            tileManager.terrainTileMap.SetTile(temp, null);//�����
        }
        else
        {
            tileManager.tileInfos[x][y].tileSlot.itemData = itemDic[_to];
            tileManager.tileInfos[x][y].tileSlot.itemCount = count;
            Vector3Int temp = tileManager.terrainTileMap.WorldToCell(new Vector3Int(x % 30, y));//���忡�� Ÿ�� ��ǥ�� ��ȯ
            tileManager.terrainTileMap.SetTile(temp, itemDic[_to].tileImg);//Ư�� ������ Ÿ�Ϸ� �׷��ֱ�
        }

    }

    #endregion
}
