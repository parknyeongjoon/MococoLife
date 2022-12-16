using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProductTerrain : Terrain, IDamagable
{
    [SerializeField] ItemData productData;//积己拱
    [SerializeField] int productCount;

    public override void Damage(float dmg)
    {
        photonView.RPC("SetHP", RpcTarget.AllBuffered, hp - dmg);
        if (hp <= 0)
        {
            SpawnProduct();//product 积魂
        }
    }

    [PunRPC]
    void SetHP(float _hp)
    {
        hp = _hp;
        if (hp <= 0 && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    void SpawnProduct()//积己且 困摹 拌魂秦辑 积己拱 积己
    {
        int tX = (int)transform.position.x;
        int tY = (int)transform.position.y;

        GameManager.Instance.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, tX, tY, productData.code, productCount);
    }
}
