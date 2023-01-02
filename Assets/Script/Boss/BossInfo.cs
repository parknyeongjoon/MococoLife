using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossInfo : MonoBehaviourPun, IDamagable, IPunObservable
{
    PhotonView PV;

    Boss_State state = Boss_State.Idle;
    [SerializeField] float hp;

    public float Hp { get => hp; set => hp = value; }
    public Boss_State State { get => state; set => state = value; }

    void Start()
    {
        PV = photonView;
    }

    public void Damage(float dmg)
    {
        if(State != Boss_State.Immune)
        {
            hp -= dmg;

            if (hp <= 0 && state != Boss_State.Dead)
            {
                GameManager.Instance.photonView.RPC("GameClear", RpcTarget.AllViaServer);///임시(코루틴이나 유니티 이벤트로 빼기)
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//정보 보내기
        {
            stream.SendNext(hp);
            stream.SendNext(state);
        }
        else//정보 받기
        {
            hp = (float)stream.ReceiveNext();
            state = (Boss_State)stream.ReceiveNext();
        }
    }
}
