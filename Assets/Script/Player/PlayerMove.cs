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

        if (info.hand.itemData == null || info.hand.itemData.Item_Type == Item_Type.BattleItem)//�տ� ��� �ִ� �� ���ų� �����̶��
        {
            if (tileSlot.itemData != null)//�ٴڿ� �������� �ִٸ� �տ� ���
            {
                info.hand.itemData = tileSlot.itemData;
                info.hand.itemCount = tileSlot.itemCount;

                tileSlot.itemData = null;
                tileSlot.itemCount = 0;
                ///�հ� tile �̹��� ����
                //Ÿ�� ���� ����, Ÿ�� �̹��� ����
                gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, "", 0);
            }
        }
        else//�տ� ��� �ִ� �� �ְ�
        {
            if (tileSlot.itemData == info.hand.itemData)//�ٴڿ� �ִ� �����۰� �տ� ��� �ִ� �������� ���ٸ�
            {
                if (info.hand.itemCount + tileSlot.itemCount > info.hand.itemData.maxCount)//�ִ� �� �� �ִ� �������� ���ٸ� �ִ��Ѹ� ���
                {
                    info.hand.itemCount = info.hand.itemData.maxCount;
                    tileSlot.itemCount = info.hand.itemCount + tileSlot.itemCount - info.hand.itemData.maxCount;
                    ///�հ� Ÿ�� �̹��� ����
                    //Ÿ�� ���� ����, Ÿ�� �̹��� ����
                    gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, tileSlot.itemData.code, tileSlot.itemCount);
                }
                else
                {
                    info.hand.itemCount = info.hand.itemCount + tileSlot.itemCount;
                    tileSlot.itemData = null;
                    tileSlot.itemCount = 0;
                    ///�հ� Ÿ�� �̹��� ����
                    /////Ÿ�� ���� ����, Ÿ�� �̹��� ����
                    gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, "", 0);
                }
            }
            else if (tileSlot.itemData != null)//�ٴڿ� �������� �ִٸ�
            {
                Slot temp = new Slot();
                temp.itemData = tileSlot.itemData;
                temp.itemCount = tileSlot.itemCount;

                tileSlot.itemData = info.hand.itemData;
                tileSlot.itemCount = info.hand.itemCount;

                info.hand = temp;
                ///�հ� Ÿ�� ��������Ʈ ����
                //Ÿ�� ���� ����, Ÿ�� �̹��� ����
                gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, tileSlot.itemData.code, tileSlot.itemCount);
            }
            else//�ٴڿ� �������� ���ٸ� �׳� ��������
            {
                tileSlot.itemData = info.hand.itemData;
                tileSlot.itemCount = info.hand.itemCount;

                info.hand.itemData = null;
                info.hand.itemCount = 0;
                ///�հ� Ÿ�� ��������Ʈ ����
                //Ÿ�� ���� ����, Ÿ�� �̹��� ����
                gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, tileSlot.itemData.code, tileSlot.itemCount);
            }
        }
        //RPC
        //�ڱ� ĳ������ �� �̹��� ����
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
