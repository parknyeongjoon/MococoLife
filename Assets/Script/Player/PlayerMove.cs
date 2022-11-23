using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviourPun
{
    PhotonView PV;
    TileManager tileManager;
    GameManager gameManager;

    [SerializeField] PlayerInfo info;
    [SerializeField] Collider2D toolCollider;

    Vector3 moveDir;
    Vector3 effectPos;

    void Start()
    {
        PV = photonView;
        gameManager = GameManager.Instance;
        tileManager = TileManager.Instance;
    }

    void Update()
    {
        if (PV.IsMine && info.state == State.Idle)
        {
            Move();
            SetToolOffset();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Dash());
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Interactive();
            }
            else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)//UI가 아닌 곳을 좌클릭했을 때
            {
                if(info.hand.itemData.Item_Type == Item_Type.Tool)
                {
                    info.hand.itemData.Effect(transform.position + effectPos);
                }
                else if (info.hand.itemData.Item_Type == Item_Type.BattleItem)
                {
                    //배템 사용 + 아이템 개수 줄이기
                }
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
        }
    }

    IEnumerator Dash()
    {
        info.state = State.Dash;

        float time = 0.0f;

        while (time <= 0.1f)
        {
            transform.position += moveDir * 10 * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }

        info.state = State.Idle;
    }

    void Interactive()//다른 거 들고 있을 때(아무것도 없을 때) 나무나 목재 들 때 체크해주기
    {
        int pX = (int)transform.position.x;
        int pY = (int)transform.position.y;

        Slot tileSlot = tileManager.tileInfos[pX][pY].tileSlot;

        if(tileSlot.itemData == null && info.hand.itemData.Item_Type != Item_Type.BattleItem)//빈칸이고 손에 든 게 배틀아이템이 아니라면
        {
            gameManager.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, pX, pY, info.hand.itemData.code, info.hand.itemCount);//아이템 집어넣고
            info.photonView.RPC("SetHand", RpcTarget.AllViaServer, gameManager.myPlayerNum, "T_00", 0);//손은 맨손으로
        }
        else if(tileSlot.itemData != null)//바닥에 아이템이 있다면
        {
            if (info.hand.itemData.code == "T_00" || info.hand.itemData.Item_Type == Item_Type.BattleItem)//빈 손이거나 배템이라면 줍기
            {
                info.photonView.RPC("SetHand", RpcTarget.AllViaServer, gameManager.myPlayerNum, tileSlot.itemData.code, tileSlot.itemCount);//줍고
                gameManager.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, pX, pY, null, 0);//바닥을 깨끗하게

            }
            else if (info.hand.itemData.Item_Type == Item_Type.Ingredient)
            {

            }
            else
            {

            }
        }
        /*
        if (info.hand.itemData.Item_Type == Item_Type.BattleItem)//손에 들고 있는 게 없거나 배템이라면
        {
            if (tileSlot.itemData != null)//바닥에 아이템이 있다면 손에 들기
            {
                info.hand.itemData = tileSlot.itemData;
                info.hand.itemCount = tileSlot.itemCount;

                tileSlot.itemData = null;
                tileSlot.itemCount = 0;
                //손과 타일 이미지 변경
                //타일 정보 변경, 타일 이미지 변경
                gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, null, 0);
                info.photonView.RPC("SetHandImg", RpcTarget.AllViaServer, gameManager.myPlayerNum, info.hand.itemData.code);
            }
        }
        else//손에 들고 있는 게 있고
        {
            if (tileSlot.itemData == info.hand.itemData)//나무나 목재라면 최대개수 체크하기
            {
                if (info.hand.itemCount + tileSlot.itemCount > info.hand.itemData.maxCount)//최대 들 수 있는 개수보다 많다면 최대한만 들기
                {
                    info.hand.itemCount = info.hand.itemData.maxCount;
                    tileSlot.itemCount = info.hand.itemCount + tileSlot.itemCount - info.hand.itemData.maxCount;
                    //손과 타일 이미지 변경
                    //타일 정보 변경, 타일 이미지 변경
                    gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, tileSlot.itemData.code, tileSlot.itemCount);
                    info.photonView.RPC("SetHandImg", RpcTarget.AllViaServer, gameManager.myPlayerNum, info.hand.itemData.code);
                }
                else
                {
                    info.hand.itemCount = info.hand.itemCount + tileSlot.itemCount;
                    tileSlot.itemData = null;
                    tileSlot.itemCount = 0;
                    //손과 타일 이미지 변경
                    //타일 정보 변경, 타일 이미지 변경
                    ///개수에 따라 이미지 다르게 변경하기
                    gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, null, 0);
                    info.photonView.RPC("SetHandImg", RpcTarget.AllViaServer, gameManager.myPlayerNum, info.hand.itemData.code);
                }
            }
            if (false) { }
            else if (tileSlot.itemData != null)//바닥에 아이템이 있다면
            {
                Slot temp = new Slot();
                temp.itemData = tileSlot.itemData;
                temp.itemCount = tileSlot.itemCount;

                tileSlot.itemData = info.hand.itemData;
                tileSlot.itemCount = info.hand.itemCount;

                info.hand = temp;
                //손과 타일 이미지 변경
                //타일 정보 변경, 타일 이미지 변경
                gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, tileSlot.itemData.code, tileSlot.itemCount);
                info.photonView.RPC("SetHandImg", RpcTarget.AllViaServer, gameManager.myPlayerNum, info.hand.itemData.code);
            }
            else//바닥에 아이템이 없다면 그냥 내려놓기
            {
                tileSlot.itemData = info.hand.itemData;
                tileSlot.itemCount = info.hand.itemCount;

                info.hand.itemData = null;
                info.hand.itemCount = 0;
                //손과 타일 이미지 변경
                //타일 정보 변경, 타일 이미지 변경
                gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, tileSlot.itemData.code, tileSlot.itemCount);
                info.photonView.RPC("SetHandImg", RpcTarget.AllViaServer, gameManager.myPlayerNum, null);
            }
        }
        */
    }

    void SwapHandTile(Slot tileSlot, Slot handSlot, int X, int Y)
    {

    }

    #endregion

    #region Animation



    #endregion

    Vector3 mousePos;

    void SetToolOffset()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        effectPos = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0).normalized * 0.3f;
        toolCollider.offset = effectPos;
    }
}
