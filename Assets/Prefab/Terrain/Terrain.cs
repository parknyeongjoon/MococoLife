using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Terrain : MonoBehaviourPun, IDamagable
{
    [SerializeField] protected float hp;
    [SerializeField] GameObject interactiveIcon;
    protected TileManager tileManager;

    public GameObject InteractiveIcon { get => interactiveIcon; }

    void Start()
    {
        int tX = (int)transform.position.x;
        int tY = (int)transform.position.y;

        tileManager = TileManager.Instance;
        //tileManager.tileInfos[tX][tY].isTerrain = true;
    }

    public virtual void Damage(float dmg)
    {

    }
}
