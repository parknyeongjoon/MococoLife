using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProductTerrain : Terrain, IDamagable
{
    [SerializeField] ItemData productData;//������
    [SerializeField] int productCount;

    public override void Damage(float dmg)
    {
        photonView.RPC("SetHP", RpcTarget.AllViaServer, hp - dmg);
    }

    [PunRPC]
    void SetHP(float _hp)
    {
        hp = _hp;
        if (hp <= 0 && PhotonNetwork.IsMasterClient)
        {
            SpawnProduct();
            PhotonNetwork.Destroy(gameObject);
        }
    }

    void SpawnProduct()//������ ��ġ ����ؼ� ������ ����
    {
        int tX = (int)transform.position.x;
        int tY = (int)transform.position.y;

        GameManager.Instance.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, tX, tY, productData.code, productCount);
    }
}
