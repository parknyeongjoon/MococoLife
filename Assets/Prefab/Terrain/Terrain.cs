using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Terrain : MonoBehaviourPun, IDamagable
{
    [SerializeField] protected Dmg_Type dmg_Type;
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

    public virtual void Damage(Dmg_Type _dmg_Type, float dmg)///�굵 RPC�� �ٲ������
    {
        if (dmg_Type == _dmg_Type)
        {
            hp -= dmg;
            if (hp <= 0)
            {
                gameObject.SetActive(false);//����ȭ �ǳ� Ȯ���غ���
            }
        }
    }
}
