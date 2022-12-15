using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossInfo : MonoBehaviourPun, IDamagable, IPunObservable
{
    PhotonView PV;

    [SerializeField] float hp;

    public float Hp { get => hp; set => hp = value; }

    void Start()
    {
        PV = photonView;
    }

    public void Damage(Dmg_Type dmg_Type, float dmg)
    {
        if (dmg_Type == Dmg_Type.Damage)
        {
            hp -= dmg;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//���� ������
        {
            stream.SendNext(hp);
        }
        else//���� �ޱ�
        {
            hp = (float)stream.ReceiveNext();
        }
    }
}
