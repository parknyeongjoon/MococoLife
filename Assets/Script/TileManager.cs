using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TileManager : MonoBehaviourPun
{
    static TileManager instance;
    public static TileManager Instance
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

    public AreaSO areas;
    public GameObject rightBoundary;

    TileInfo[][] tileInfos;
    public int tileCount;

    IEnumerator Start()
    {
        yield return SetGame();
    }

    IEnumerator SetGame()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        RPCSetGameArea();
    }

    public void RPCSetGameArea()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int[] tiles = new int[tileCount];

            for (int i = 0; i < tileCount; i++)
            {
                tiles[i] = Random.Range(0, areas.bossAreas.Count);
            }

            GameManager.Instance.PV.RPC("SetGameArea", RpcTarget.AllBufferedViaServer, tiles);
        }
    }
}
