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

    void Awake()
    {
        instance = this;
    }

    public int[] AreaInitialize()
    {
        int[] tiles = new int[tileCount];

        for (int i = 0; i < tileCount; i++)
        {
            tiles[i] = Random.Range(0, areas.bossAreas.Count);
        }

        return tiles;
    }
}
