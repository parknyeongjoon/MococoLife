using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProductTerrain : Terrain
{
    [SerializeField] ItemData productData;//积己拱
    [SerializeField] int productCount;

    public override void Damage(Dmg_Type _dmg_Type, float dmg)
    {
        if (dmg_Type == _dmg_Type)
        {
            hp -= dmg;
            if (hp <= 0)
            {
                //product 积魂
                SpawnProduct();
            }
        }
    }

    void SpawnProduct()//积己且 困摹 拌魂秦辑 积己拱 积己
    {
        int tX = (int)transform.position.x;
        int tY = (int)transform.position.y;

        GameManager.Instance.photonView.RPC("SetTileItem", Photon.Pun.RpcTarget.AllViaServer, tX, tY, productData.code, productCount);
        transform.position = new Vector3(-30, 0, 0);
    }
}
