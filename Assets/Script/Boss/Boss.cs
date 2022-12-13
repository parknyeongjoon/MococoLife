using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Boss : MonoBehaviourPun, IDamagable, IPunObservable
{
    PhotonView PV;

    [SerializeField] float hp;

    public float Hp { get => hp; }

    void Start()
    {
        PV = photonView;
    }

    #region Pattern

    #endregion

    public void Damage(Dmg_Type dmg_Type, float dmg)
    {
        if (dmg_Type == Dmg_Type.Damage)
        {
            hp -= dmg;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //피 동기화
    }
}
