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

    PhotonView PV;

    public AreaSO areas;
    [SerializeField] GameObject rightBoundary;

    TileInfo[][] tileInfos;
    public int tileCount;

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

            PV.RPC("SetGameArea", RpcTarget.AllBufferedViaServer, tiles);
        }
    }

    [PunRPC]
    void SetGameArea(int[] tiles)//photonview를 달아가면서까지 PhotonNetwork.Instantiate를 사용할 필요가 있을까?
    {
        for (int i = 0; i < tileCount; i++)
        {
            Instantiate(areas.bossAreas[tiles[i]], new Vector3(30 * i, 0, 0), Quaternion.identity);
        }

        GameObject.Find("RightBoundary").transform.position = new Vector3(30 * tileCount - 15, 0, 0);
    }
}
