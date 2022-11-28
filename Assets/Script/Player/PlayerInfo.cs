using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : MonoBehaviourPun
{
    GameManager gameManager;
    public PlayerMove playerMove;

    float hp = 100;
    State state;
    [SerializeField] Slot hand = new Slot();//�ø��� �����
    [SerializeField] Slot[] inventory = new Slot[4];//�ø��� �����
    public SpriteRenderer handImg;

    public State State { get => state; set => state = value; }/// �̰ŵ� ���� �������� set�� ���ְ� ������ ��� ã��
    public Slot Hand { get => hand; }
    public float Hp { get; set; }
    public Slot[] Inventory { get => inventory; set => inventory = value; }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        photonView.RPC("SetPlayers", RpcTarget.AllBufferedViaServer, gameManager.MyPlayerNum);
        hand.itemData = gameManager.itemDic["T_00"];
    }

    [PunRPC] void SetPlayers(int index)
    {
        gameManager.players[index] = this;
    }

    [PunRPC] public void SetHand(int index, string code, int count)
    {
        if(gameManager.itemDic[code].Item_Type == Item_Type.Ingredient && count == 0)
        {
            code = "T_00";
        }
        Hand.itemData = gameManager.itemDic[code];
        Hand.itemCount = count;
        gameManager.players[index].handImg.sprite = gameManager.itemDic[code].itemImg;
    }
}
