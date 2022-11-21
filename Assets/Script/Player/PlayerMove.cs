using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    PhotonView PV;
    TileManager tileManager;
    GameManager gameManager;

    [SerializeField] PlayerInfo info;

    Vector3 moveDir;

    void Start()
    {
        PV = photonView;
        gameManager = GameManager.Instance;
        tileManager = TileManager.Instance;
    }

    void Update()
    {
        if (PV.IsMine)
        {
            Move();

            if (info.state == State.Idle && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Dash());
            }

            if (info.state == State.Idle && Input.GetKeyDown(KeyCode.E))
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
            moveDir.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDir.y = -1;
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
            moveDir.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDir.x = 1;
        }
        else
        {
            moveDir.x = 0;
        }

        if (info.state == State.Idle && moveDir != Vector3.zero)
        {
            transform.position += moveDir * 2 * Time.deltaTime;
            //toolCollider.offset = new Vector2(moveDir.x, moveDir.y);
        }
    }

    IEnumerator Dash()
    {
        info.state = State.Dash;

        float time = 0.0f;

        while (time <= 0.2f)
        {
            transform.position += moveDir * 5 * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }

        info.state = State.Idle;
    }

    [PunRPC]
    void Interactive()
    {
        int pX = (int)transform.position.x;
        int pY = (int)transform.position.y;

        Slot tileSlot = tileManager.tileInfos[pX][pY].tileSlot;

        if (info.hand.itemData == null || info.hand.itemData.Item_Type == Item_Type.BattleItem)//손에 들고 있는 게 없거나 배템이라면
        {
            if (tileSlot.itemData != null)//바닥에 아이템이 있다면 손에 들기
            {
                info.hand.itemData = tileSlot.itemData;
                info.hand.itemCount = tileSlot.itemCount;

                tileSlot.itemData = null;
                tileSlot.itemCount = 0;
                ///손과 tile 이미지 변경
                //타일 정보 변경, 타일 이미지 변경
                gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, "", 0);
            }
        }
        else//손에 들고 있는 게 있고
        {
            if (tileSlot.itemData == info.hand.itemData)//바닥에 있는 아이템과 손에 들고 있는 아이템이 같다면
            {
                if (info.hand.itemCount + tileSlot.itemCount > info.hand.itemData.maxCount)//최대 들 수 있는 개수보다 많다면 최대한만 들기
                {
                    info.hand.itemCount = info.hand.itemData.maxCount;
                    tileSlot.itemCount = info.hand.itemCount + tileSlot.itemCount - info.hand.itemData.maxCount;
                    ///손과 타일 이미지 변경
                    //타일 정보 변경, 타일 이미지 변경
                    gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, tileSlot.itemData.code, tileSlot.itemCount);
                }
                else
                {
                    info.hand.itemCount = info.hand.itemCount + tileSlot.itemCount;
                    tileSlot.itemData = null;
                    tileSlot.itemCount = 0;
                    ///손과 타일 이미지 변경
                    /////타일 정보 변경, 타일 이미지 변경
                    gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, "", 0);
                }
            }
            else if (tileSlot.itemData != null)//바닥에 아이템이 있다면
            {
                Slot temp = new Slot();
                temp.itemData = tileSlot.itemData;
                temp.itemCount = tileSlot.itemCount;

                tileSlot.itemData = info.hand.itemData;
                tileSlot.itemCount = info.hand.itemCount;

                info.hand = temp;
                ///손과 타일 스프라이트 변경
                //타일 정보 변경, 타일 이미지 변경
                gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, tileSlot.itemData.code, tileSlot.itemCount);
            }
            else//바닥에 아이템이 없다면 그냥 내려놓기
            {
                tileSlot.itemData = info.hand.itemData;
                tileSlot.itemCount = info.hand.itemCount;

                info.hand.itemData = null;
                info.hand.itemCount = 0;
                ///손과 타일 스프라이트 변경
                //타일 정보 변경, 타일 이미지 변경
                gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, tileSlot.itemData.code, tileSlot.itemCount);
            }
        }
        //RPC
        //자기 캐릭터의 손 이미지 변경
        //photonView.RPC()
    }

    void UseItem(int index, Vector2Int effectPos)
    {
        info.inventory[index].itemData.Effect(effectPos);
    }

    #endregion

    #region Animation


    #endregion


}
