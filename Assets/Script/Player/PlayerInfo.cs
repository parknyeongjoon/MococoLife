using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : MonoBehaviourPun
{
    GameManager gameManager;
    public PlayerMove playerMove;

    public float hp = 100;
    public State state;
    public Slot hand = new Slot();
    public Slot[] inventory = new Slot[4];
    public SpriteRenderer handImg;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    public void SetHandImg(int index, string code)
    {
        if (code == null)
        {
            gameManager.players[index].handImg.sprite = null;
        }
        else
        {
            gameManager.players[index].handImg.sprite = gameManager.itemDic[code].itemImg;
        }
    }
}
