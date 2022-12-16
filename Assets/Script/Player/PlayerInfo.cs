using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : MonoBehaviourPun, IDamagable, IPunObservable
{
    GameManager gameManager;
    public PlayerBehaviour playerMove;

    [SerializeField] float hp = 100;

    public float canAtkTime;

    P_State state;
    [SerializeField] Slot hand = new Slot();
    Slot[] inventory = new Slot[4];

    public P_State State { get => state; set => state = value; }
    public Slot Hand { get => hand; }
    public float Hp { get => hp; }
    public Slot[] Inventory { get => inventory; set => inventory = value; }

    #region rpcVar

    public GameObject LAreaMoveIcon, RAreaMoveIcon, WarningIcon;
    public SpriteRenderer handImg;

    #endregion

    void Awake()
    {
        gameManager = GameManager.Instance;

        if (photonView.IsMine)
        {
            photonView.RPC("SetPlayers", RpcTarget.AllBufferedViaServer, gameManager.MyPlayerNum);
        }

        hand.itemData = gameManager.itemDic["T_00"];

        for (int i = 0; i < 4; i++)
        {
            inventory[i] = new Slot();
        }
    }

    [PunRPC]
    void SetPlayers(int index)
    {
        gameManager.players[index] = this;
    }

    [PunRPC]
    public void SetHand(int index, string code, int count)///이거 버그 일어날 거 같은데? - get만 넣어놔서 안 나는듯?
    {
        Hand.itemData = gameManager.itemDic[code];
        Hand.itemCount = count;
        gameManager.players[index].handImg.sprite = gameManager.itemDic[code].itemImg;
    }

    public void Damage(float dmg)
    {
        hp -= dmg;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//정보 보내기
        {
            stream.SendNext(hp);
        }
        else//정보 받기
        {
            hp = (float)stream.ReceiveNext();
        }
    }
}
