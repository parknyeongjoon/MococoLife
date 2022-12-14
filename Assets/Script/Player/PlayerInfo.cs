using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : MonoBehaviourPun, IDamagable, IPunObservable
{
    GameManager gameManager;
    public PlayerMove playerMove;

    [SerializeField] float hp = 100;

    public float canAtkTime;

    State state;
    [SerializeField] Slot hand = new Slot();
    Slot[] inventory = new Slot[4];

    public State State { get => state; set => state = value; }///�̰ŵ� ���� �������� set�� ���ְ� ����
    public Slot Hand { get => hand; }
    public float Hp { get; set; }
    public Slot[] Inventory { get => inventory; set => inventory = value; }

    #region rpcVar

    public GameObject LAreaMoveIcon, RAreaMoveIcon;
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

    [PunRPC] void SetPlayers(int index)
    {
        gameManager.players[index] = this;
    }

    [PunRPC] public void SetHand(int index, string code, int count)///�̰� ���� �Ͼ �� ������? - get�� �־���� �� ���µ�?
    {
        Hand.itemData = gameManager.itemDic[code];
        Hand.itemCount = count;
        gameManager.players[index].handImg.sprite = gameManager.itemDic[code].itemImg;
    }

    public void Damage(Dmg_Type dmg_Type, float dmg)
    {
        if(dmg_Type == Dmg_Type.Damage)
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
            Debug.Log(hp);
        }
    }
}
