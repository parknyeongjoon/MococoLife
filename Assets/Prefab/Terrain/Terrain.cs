using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Terrain : MonoBehaviourPun, IDamagable
{
    [SerializeField] protected Dmg_Type dmg_Type;
    [SerializeField] protected float hp;

    public virtual void Damage(Dmg_Type _dmg_Type, float dmg)
    {
        if (dmg_Type == _dmg_Type)
        {
            hp -= dmg;
            if (hp <= 0)
            {
                gameObject.SetActive(false);//동기화 되나 확인해보기
            }
        }
    }
}
