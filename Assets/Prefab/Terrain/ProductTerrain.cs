using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductTerrain : Terrain
{
    [SerializeField] GameObject product;//�λ깰
    [SerializeField] int productCount;
    int remainProduct;
    float productHP;

    void Start()
    {
        remainProduct = productCount - 1;
        productHP = hp / productCount * remainProduct;
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

                if (hp <= 0)
                {
                    gameObject.SetActive(false);//����ȭ �ǳ� Ȯ���غ���
                }
            }
        }
    }
}
