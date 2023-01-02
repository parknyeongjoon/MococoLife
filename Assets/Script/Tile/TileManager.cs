using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    GameManager gameManager;

    public AreaSO areas;

    public int areaCount;
    public TileInfo[][] tileInfos;
    public Tilemap terrainTileMap;

    void Awake()
    {
        gameManager = GameManager.Instance;

        tileInfos = new TileInfo[30 * areaCount][];
        for (int i = 0; i < 30 * areaCount; i++)
        {
            tileInfos[i] = new TileInfo[14];
            for(int j = 0; j < 14; j++)
            {
                tileInfos[i][j] = new TileInfo();
            }
        }

        instance = this;
    }

    void Start()
    {
        Pooler.Instance.SetPhotonPool(areas.bossAreas);
    }

    public int[] AreaInitialize()
    {
        int[] tiles = new int[areaCount];

        for (int i = 1; i < areaCount; i++)
        {
            tiles[i] = Random.Range(0, areas.bossAreas.Count);
        }

        return tiles;
    }
}
