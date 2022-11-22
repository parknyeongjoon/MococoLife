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
            else if (Input.GetMouseButtonDown(0))
            {
                info.hand.itemData.Effect(transform.position + effectPos);
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

    [PunRPC]
    void Interactive()//�ٸ� �� ��� ���� ��(�ƹ��͵� ���� ��) ������ ���� �� �� üũ���ֱ�
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
                //�հ� Ÿ�� �̹��� ����
                //Ÿ�� ���� ����, Ÿ�� �̹��� ����
                gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, null, 0);
                info.photonView.RPC("SetHandImg", RpcTarget.AllViaServer, gameManager.myPlayerNum, info.hand.itemData.code);
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
                    //�հ� Ÿ�� �̹��� ����
                    //Ÿ�� ���� ����, Ÿ�� �̹��� ����
                    gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, tileSlot.itemData.code, tileSlot.itemCount);
                    info.photonView.RPC("SetHandImg", RpcTarget.AllViaServer, gameManager.myPlayerNum, info.hand.itemData.code);
                }
                else
                {
                    info.hand.itemCount = info.hand.itemCount + tileSlot.itemCount;
                    tileSlot.itemData = null;
                    tileSlot.itemCount = 0;
                    //�հ� Ÿ�� �̹��� ����
                    //Ÿ�� ���� ����, Ÿ�� �̹��� ����
                    ///������ ���� �̹��� �ٸ��� �����ϱ�
                    gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, null, 0);
                    info.photonView.RPC("SetHandImg", RpcTarget.AllViaServer, gameManager.myPlayerNum, info.hand.itemData.code);
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
                //�հ� Ÿ�� �̹��� ����
                //Ÿ�� ���� ����, Ÿ�� �̹��� ����
                gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, tileSlot.itemData.code, tileSlot.itemCount);
                info.photonView.RPC("SetHandImg", RpcTarget.AllViaServer, gameManager.myPlayerNum, info.hand.itemData.code);
            }
            else//�ٴڿ� �������� ���ٸ� �׳� ��������
            {
                tileSlot.itemData = info.hand.itemData;
                tileSlot.itemCount = info.hand.itemCount;

                info.hand.itemData = null;
                info.hand.itemCount = 0;
                //�հ� Ÿ�� �̹��� ����
                //Ÿ�� ���� ����, Ÿ�� �̹��� ����
                gameManager.photonView.RPC("ChangeTile", RpcTarget.AllViaServer, pX, pY, tileSlot.itemData.code, tileSlot.itemCount);
                info.photonView.RPC("SetHandImg", RpcTarget.AllViaServer, gameManager.myPlayerNum, null);
            }
        }
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
