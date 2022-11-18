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
        if(hand.itemData.Item_Type != Item_Type.BattleItem)//�տ� �ִ� �� ������ �ƴ϶��
        {
            int pX = (int)transform.position.x;
            int pY = (int)transform.position.y;

            Slot tileSlot = tileManager.tileInfos[pX][pY].tileSlot;

            if (tileSlot.itemData != null)//�ٴڿ� �������� �ִٸ� �տ� �ִ� �Ͱ� �ٲٱ�
            {
                Slot temp = tileSlot;
                tileSlot = hand;
                hand = temp;
                ///ĳ���� �հ� Ÿ�� ��������Ʈ ����
            }
            else if(tileSlot.itemData == hand.itemData)//�ٴڿ� �ִ� �����۰� �տ� ��� �ִ� �������� ���ٸ�
            {
                //������ ���̶�� ��ġ��
                if (hand.itemData.code == "I_00" || hand.itemData.code == "I_01")//����(01)�� ��(01)�̶��
                {
                    if (hand.itemCount + tileSlot.itemCount > hand.itemData.maxCount)//�ִ� �� �� �ִ� �������� ���ٸ�
                    {
                        hand.itemCount = hand.itemData.maxCount;
                        tileSlot.itemCount = hand.itemCount + tileSlot.itemCount - hand.itemData.maxCount;
                        ///ĳ���� �հ� Ÿ�� ��������Ʈ ����
                    }
                    else
                    {
                        hand.itemCount = hand.itemCount + tileSlot.itemCount;
                        tileSlot.itemData = null;
                        tileSlot.itemCount = 0;
                    }
                }
            }
            else//�ٴڿ� �������� ���ٸ� �׳� ��������
            {
                tileSlot = hand;
                hand.itemData = null;
                hand.itemCount = 0;
                ///ĳ���� �հ� Ÿ�� ��������Ʈ ����
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
