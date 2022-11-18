using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    PhotonView PV;
    TileManager tileManager;

    float hp = 100;
    State state;
    Slot hand = new Slot();
    Vector3 moveDir;

    Slot[] inventory = new Slot[4];

    [SerializeField] CircleCollider2D toolCollider;

    void Start()
    {
        PV = photonView;
        tileManager = TileManager.Instance;
    }

    void Update()
    {
        if (PV.IsMine)
        {
            Move();

            if (state == State.Idle && Input.GetKey(KeyCode.Space))
            {
                StartCoroutine(Dash());
            }

            if (state == State.Idle && Input.GetKey(KeyCode.E))
            {
                Interactive();
            }
        }
    }
    #region Action

    void Move()
    {

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            moveDir.y = 0;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            moveDir.y = 2;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDir.y = -2;
        }
        else
        {
            moveDir.y = 0;
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            moveDir.x = 0;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveDir.x = -2;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDir.x = 2;
        }
        else
        {
            moveDir.x = 0;
        }

        if (state == State.Idle && moveDir != Vector3.zero)
        {
            transform.position += moveDir * Time.deltaTime;
            toolCollider.offset = new Vector2(moveDir.x, moveDir.y);
        }
    }

    IEnumerator Dash()
    {
        state = State.Dash;

        float time = 0.0f;

        while (time <= 0.2f)
        {
            transform.position += moveDir * 3 * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }

        state = State.Idle;
    }

    void Interactive()
    {
        if(hand.itemData.Item_Type != Item_Type.BattleItem)//손에 있는 게 배템이 아니라면
        {
            int pX = (int)transform.position.x;
            int pY = (int)transform.position.y;

            Slot tileSlot = tileManager.tileInfos[pX][pY].tileSlot;

            if (tileSlot.itemData != null)//바닥에 아이템이 있다면 손에 있는 것과 바꾸기
            {
                Slot temp = tileSlot;
                tileSlot = hand;
                hand = temp;
                ///캐릭터 손과 타일 스프라이트 변경
            }
            else if(tileSlot.itemData == hand.itemData)//바닥에 있는 아이템과 손에 들고 있는 아이템이 같다면
            {
                //나무나 돌이라면 겹치기
                if (hand.itemData.code == "I_00" || hand.itemData.code == "I_01")//나무(01)나 돌(01)이라면
                {
                    if (hand.itemCount + tileSlot.itemCount > hand.itemData.maxCount)//최대 들 수 있는 개수보다 많다면
                    {
                        hand.itemCount = hand.itemData.maxCount;
                        tileSlot.itemCount = hand.itemCount + tileSlot.itemCount - hand.itemData.maxCount;
                        ///캐릭터 손과 타일 스프라이트 변경
                    }
                    else
                    {
                        hand.itemCount = hand.itemCount + tileSlot.itemCount;
                        tileSlot.itemData = null;
                        tileSlot.itemCount = 0;
                    }
                }
            }
            else//바닥에 아이템이 없다면 그냥 내려놓기
            {
                tileSlot = hand;
                hand.itemData = null;
                hand.itemCount = 0;
                ///캐릭터 손과 타일 스프라이트 변경
            }
        }
    }

    #endregion

    #region Item

    void UseItem(int index, Vector2Int effectPos)
    {
        inventory[index].itemData.Effect(effectPos);
    }

    #endregion
}
