using Photon.Pun;
using UnityEngine;

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

    public int areaCount;
    public TileInfo[][] tileInfos;

    void Awake()
    {
        instance = this;
        tileInfos = new TileInfo[30 * areaCount][];
    }

    public int[] AreaInitialize()
    {
        int[] tiles = new int[areaCount];

        for (int i = 0; i < areaCount; i++)
        {
            tiles[i] = Random.Range(0, areas.bossAreas.Count);
        }

        return tiles;
    }
}
