using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductTerrain : Terrain
{
    [SerializeField] GameObject product;//부산물
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
                //product 생산
                remainProduct -= 1;
                productHP = hp / productCount * remainProduct;

                if (hp <= 0)
                {
                    gameObject.SetActive(false);//동기화 되나 확인해보기
                }
            }
        }
    }
}
