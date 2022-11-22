using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                gameObject.SetActive(false);//����ȭ �ǳ� Ȯ���غ���
            }
        }
    }

    void SpawnProduct()//������ ��ġ ����ؼ� ������ ����
    {
        int tX = (int)transform.position.x;
        int tY = (int)transform.position.y;

        //tileManager.tileInfos[tX][tY].isTerrain = false;
        GameManager.Instance.photonView.RPC("ChangeTile", Photon.Pun.RpcTarget.AllViaServer, tX, tY, productData.code, productCount);
    }
}
