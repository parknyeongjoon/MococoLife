using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductTerrain : Terrain
{
    PoolManager poolManager;

    [SerializeField] ItemData productData;//������
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
                //product ����
                remainProduct -= 1;
                productHP = hp / productCount * remainProduct;
                SpawnProduct();

                if (hp <= 0)
                {
                    gameObject.SetActive(false);//����ȭ �ǳ� Ȯ���غ���
                }
            }
        }
    }

    void SpawnProduct()//������ ��ġ ����ؼ� ������ ����
    {
        poolManager.Get(productData.code, transform.position - new Vector3(-1, 0, 0));
    }
}
