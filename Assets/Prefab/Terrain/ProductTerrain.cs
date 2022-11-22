using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductTerrain : Terrain
{
    [SerializeField] ItemData productData;//생성물
    [SerializeField] int productCount;

    public override void Damage(Dmg_Type _dmg_Type, float dmg)
    {
        if (dmg_Type == _dmg_Type)
        {
            hp -= dmg;
            if (hp <= 0)
            {
                //product 생산
                SpawnProduct();
                gameObject.SetActive(false);//동기화 되나 확인해보기
            }
        }
    }

    void SpawnProduct()//생성할 위치 계산해서 생성물 생성
    {
        int tX = (int)transform.position.x;
        int tY = (int)transform.position.y;

        //tileManager.tileInfos[tX][tY].isTerrain = false;
        GameManager.Instance.photonView.RPC("ChangeTile", Photon.Pun.RpcTarget.AllViaServer, tX, tY, productData.code, productCount);
    }
}
