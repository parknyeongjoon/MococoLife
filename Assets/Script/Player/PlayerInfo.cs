using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : MonoBehaviourPun, IDamagable
{
    GameManager gameManager;
    public PlayerMove playerMove;

    [SerializeField] float hp = 100;
    public bool canAtk;

    State state;
    Slot hand = new Slot();
    Slot[] inventory = new Slot[4];
    public SpriteRenderer handImg;

    public State State { get => state; set => state = value; }/// 이거도 보안 목적으로 set을 없애고 싶은데 방법 찾기
    public Slot Hand { get => hand; }
    public float Hp { get; set; }
    public Slot[] Inventory { get => inventory; set => inventory = value; }

    void Awake()
    {
        gameManager = GameManager.Instance;

        if (photonView.IsMine)
        {
            photonView.RPC("SetPlayers", RpcTarget.AllBufferedViaServer, gameManager.MyPlayerNum);
        }
        hand.itemData = gameManager.itemDic["T_00"];
        for(int i = 0; i < 4; i++)
        {
            inventory[i] = new Slot();
        }
    }

    [PunRPC] void SetPlayers(int index)
    {
        gameManager.players[index] = this;
    }

    [PunRPC] public void SetHand(int index, string code, int count)
    {
        Hand.itemData = gameManager.itemDic[code];
        Hand.itemCount = count;
        gameManager.players[index].handImg.sprite = gameManager.itemDic[code].itemImg;
    }

    public IEnumerator EatFood()
    {
        canAtk = true;
        yield return new WaitForSeconds(3.0f);
        canAtk = false;
    }

    public void Damage(Dmg_Type dmg_Type, float dmg)
    {
        if(dmg_Type == Dmg_Type.Damage)
        {
            hp -= dmg;
        }
    }
}
