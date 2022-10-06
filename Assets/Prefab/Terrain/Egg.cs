using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Egg : MonoBehaviourPun, IDamagable
{
    [SerializeField] Dmg_Type dmg_Type;
    [SerializeField] float hp;

    public void Damage(Dmg_Type _dmg_Type, float dmg)
    {
        if(dmg_Type == _dmg_Type)
        {
            hp -= dmg;
            if(hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
