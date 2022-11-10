using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductTerrain : Terrain
{
    PoolManager poolManager;

    [SerializeField] ItemData productData;//생성물
    [SerializeField] int productCount;
    int remainProduct;
    float productHP;

    void Start()
    {
        remainProduct = productCount - 1;
        productHP = hp / productCount * remainProduct;
        poolManager = PoolManager.Instance;
    }

    public override void Damage(Dmg_Type _dmg_Type, float dmg)
    {
        if (dmg_Type == _dmg_Type)
        {
            hp -= dmg;
            if(hp < productHP)
            {
                //product 생산
                remainProduct -= 1;
                productHP = hp / productCount * remainProduct;
                SpawnProduct();

                if (hp <= 0)
                {
                    gameObject.SetActive(false);//동기화 되나 확인해보기
                }
            }
        }
    }

    void SpawnProduct()//생성할 위치 계산해서 생성물 생성
    {
        poolManager.Get(productData.code, transform.position - new Vector3(-1, 0, 0));
    }
}
