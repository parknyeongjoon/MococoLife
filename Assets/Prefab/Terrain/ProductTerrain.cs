using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProductTerrain : Terrain
{
    [SerializeField] ItemData productData;//������
    [SerializeField] int productCount;

    public override void Damage(Dmg_Type _dmg_Type, float dmg)
    {
        if (dmg_Type == _dmg_Type)
        {
            hp -= dmg;
            if (hp <= 0)
            {
                //product ����
                SpawnProduct();
            }
        }
    }

    void SpawnProduct()//������ ��ġ ����ؼ� ������ ����
    {
        int tX = (int)transform.position.x;
        int tY = (int)transform.position.y;

        GameManager.Instance.photonView.RPC("SetTileItem", Photon.Pun.RpcTarget.AllViaServer, tX, tY, productData.code, productCount);
        transform.position = new Vector3(-30, 0, 0);
    }
}
