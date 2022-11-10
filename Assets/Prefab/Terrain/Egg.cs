using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Egg : Terrain
{

    public override void Damage(Dmg_Type _dmg_Type, float dmg)
    {
        if(dmg_Type == _dmg_Type)
        {
            hp -= dmg;
            if(hp <= 0)
            {
                gameObject.SetActive(false);//동기화 되나 확인해보기
                //깨질 때 보상넣기
            }
        }
    }
}
