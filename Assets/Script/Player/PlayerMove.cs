using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
///키 입력 Dictionary<Keycode, Action>으로 넣어두고 실행하는 거 알아보기(최적화 및 가독성)
///대시 쿨타임, 액션 스테이트 만들기
///getItem이랑 useInventory는 다른데로 넘기기?
public class PlayerMove : MonoBehaviourPun
{
    PhotonView PV;
    TileManager tileManager;
    GameManager gameManager;

    [SerializeField] PlayerInfo info;
    [SerializeField] Animator animator;
    [SerializeField] Collider2D bodyCollider, toolCollider;
    [SerializeField] SpriteRenderer spriteRenderer;

    Vector3 moveDir;
    Vector3 effectPos;
    Vector3 mousePos;

    int inventory_Index;

    float dashCool;

    void Start()
    {
        PV = photonView;
        gameManager = GameManager.Instance;
        tileManager = TileManager.Instance;
    }

    void Update()
    {
        if (PV.IsMine && info.State == State.Idle)
        {
            Move();
            SetToolOffset();

            if (Input.GetKeyDown(KeyCode.Space) && moveDir != Vector3.zero && dashCool <= 0)
            {
                StartCoroutine(Dash());
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Interactive();
            }
            else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)//UI�� �ƴ� ���� ��Ŭ������ ��
            {
                if (info.Hand.itemData.Item_Type == Item_Type.Tool)
                {
                    StartCoroutine(UseItem(info.Hand.itemData, transform.position + effectPos));
                }
                else if (info.Hand.itemData.Item_Type == Item_Type.BattleItem)
                {
                    UseInventory(inventory_Index);
                }
                else if (info.Hand.itemData.Item_Type == Item_Type.Ingredient)
                {
                    Collider2D isSmith = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("BlackSmith"));

                    if (Physics2D.IsTouchingLayers(bodyCollider, 1 << LayerMask.NameToLayer("BlackSmith")) && isSmith)//BlackSmith�� ���� ���̰� ���콺 �����Ϳ� �������̽��� �ִٸ�
                    {
                        BlackSmith smith = isSmith.GetComponentInParent<BlackSmith>();
                        if (smith.Create_State == Create_State.create)
                        {
                            int result = smith.PlusIngredient(info.Hand.itemData.code, info.Hand.itemCount);
                            info.SetHand(gameManager.MyPlayerNum, info.Hand.itemData.code, info.Hand.itemCount - result);
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1) && EventSystem.current.IsPointerOverGameObject() == false)//UI�� �ƴ� ���� ��Ŭ������ ��
            {
                Collider2D isSmith = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("BlackSmith"));
                if (Physics2D.IsTouchingLayers(bodyCollider, 1 << LayerMask.NameToLayer("BlackSmith")) && isSmith)//BlackSmith�� ���� ���̰� ���콺 �����Ϳ� �������̽��� �ִٸ�
                {
                    BlackSmith smith = isSmith.GetComponentInParent<BlackSmith>();
                    if (smith.Create_State == Create_State.idle) { smith.SetCreatePanel(); }//idle ���¶�� createPanel ����
                    else if (smith.Create_State == Create_State.finish)//���� ������ �ϼ��ƴٸ� �κ��丮�� �ֱ�
                    {
                        GetItem(smith);
                    }
                }
            }
            //�κ��丮���� ������ �����ϱ�
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetItem(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetItem(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetItem(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetItem(3);
            }
        }
    }

    void SetItem(int index)
    {
        if(info.Inventory[index].itemData != null)
        {
            if(info.Hand.itemData.code == "T_00" || info.Hand.itemData.Item_Type == Item_Type.BattleItem)
            {
                inventory_Index = index;
                PV.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, info.Inventory[index].itemData.code, info.Inventory[index].itemCount);
            }
            else
            {
                int pX = (int)transform.position.x, pY = (int)transform.position.y;
                int[] result = CheckAround(pX, pY);
                if (result == null)
                {
                    Debug.Log("주변 공간 없음");
                }
                else
                {
                    inventory_Index = index;
                    gameManager.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, result[0], result[1], info.Hand.itemData.code, info.Hand.itemCount);//������ ��������
                    PV.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, info.Inventory[index].itemData.code, info.Inventory[index].itemCount);//�տ��� ���۵��
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
            if (spriteRenderer.flipX == false)
            {
                PV.RPC("Flip", RpcTarget.All);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDir.x = 1;
            if (spriteRenderer.flipX == true)
            {
                PV.RPC("Flip", RpcTarget.All);
            }
        }
        else
        {
            moveDir.x = 0;
        }

        if (info.State == State.Idle && moveDir != Vector3.zero)
        {
            transform.position += moveDir * 2 * Time.deltaTime;
            animator.SetBool("isMove", true);
        }
        else if (moveDir == Vector3.zero)
        {
            animator.SetBool("isMove", false);
        }
    }

    IEnumerator Dash()
    {
        info.State = State.Dash;

        float time = 0.0f;
        PV.RPC("AnimTrigger", RpcTarget.AllViaServer, "roll");

        while (time <= 0.05f)
        {
            transform.position += moveDir * 15 * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }

        info.State = State.Idle;

        dashCool = 3.0f;

        while(dashCool > 0)
        {
            dashCool -= Time.deltaTime;
            yield return null;
        }
    }

    void Interactive()
    {
        int pX = (int)transform.position.x;
        int pY = (int)transform.position.y;

        Slot tileSlot = tileManager.tileInfos[pX][pY].tileSlot;

        if (tileSlot.itemData == null)//��ĭ�̸�
        {
            gameManager.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, pX, pY, info.Hand.itemData.code, info.Hand.itemCount);//������ ��������
            PV.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, "T_00", 0);//���� �Ǽ�����
        }
        else if (tileSlot.itemData.Item_Type == Item_Type.Ingredient)//�ٴڿ� ��ᰡ �ְ�
        {
            if (tileSlot.itemData == info.Hand.itemData)//���� ����� ��ġ��
            {
                gameManager.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, pX, pY, info.Hand.itemData.code, tileSlot.itemCount + info.Hand.itemCount);//������ ��ġ��
                PV.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, "T_00", 0);//���� �Ǽ�����
            }
            else if (tileSlot.itemCount > ((Ingredient)tileSlot.itemData).maxCount)//�ִ� ���� ���� ���ٸ� �ֺ��� �տ� ��� �ִ� ������ ���ΰ� ���
            {
                int[] result = CheckAround(pX, pY);
                if (result == null)//�ֺ��� ���� ���� ���ٸ�
                {
                    Debug.Log("���� ���� �����ϴ�");
                }
                else//���� ���� �ִٸ�
                {
                    gameManager.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, result[0], result[1], info.Hand.itemData.code, info.Hand.itemCount);//������ ��������
                    gameManager.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, pX, pY, tileSlot.itemData.code, tileSlot.itemCount - ((Ingredient)tileSlot.itemData).maxCount);//Ÿ�Ͽ� ������ ���� ���̰�
                    PV.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, tileSlot.itemData.code, ((Ingredient)tileSlot.itemData).maxCount);//�տ��� ��� ���
                }
            }
            else//�ִ� �������� Ÿ�Ͽ� �ִ� ��ᰡ ���ٸ�
            {
                Slot temp = new Slot(); temp.itemData = tileSlot.itemData; temp.itemCount = tileSlot.itemCount;//���ҿ� ����
                gameManager.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, pX, pY, info.Hand.itemData.code, info.Hand.itemCount);//������ ��������
                PV.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, temp.itemData.code, temp.itemCount);//�ݱ�
            }
        }
        else if (tileSlot.itemData != null)//�ٴڿ� ��ᰡ �ƴ� �������� �ִٸ�
        {
            Slot temp = new Slot(); temp.itemData = tileSlot.itemData; temp.itemCount = tileSlot.itemCount;//���ҿ� ����
            gameManager.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, pX, pY, info.Hand.itemData.code, info.Hand.itemCount);//������ ��������
            PV.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, temp.itemData.code, temp.itemCount);//�ݱ�
        }

    }

    void GetItem(BlackSmith smith)//������� true �� ������� false
    {
        for (int i = 0; i < info.Inventory.Length; i++)
        {
            if (info.Inventory[i].itemData == smith.CreatingItem)//�κ��丮�� ��ġ�� �������� �ִٸ� üũ�ϱ�
            {
                if (info.Inventory[i].itemCount >= smith.CreatingItem.maxCount)
                {
                    Debug.Log("�� �̻� ���� �� �����ϴ�.");
                    return;
                }
                else
                {
                    info.Inventory[i].itemCount++;
                    smith.photonView.RPC("PickUp", RpcTarget.AllViaServer);//�������� ����ٸ� PickUp����
                    return;
                }
            }
        }
        for (int i = 0; i < info.Inventory.Length; i++)//�κ��丮�� ��ġ�� �������� ���ٸ� �� ĭ üũ�ϱ�
        {
            if (info.Inventory[i].itemData == null)
            {
                info.Inventory[i].itemData = smith.CreatingItem;
                info.Inventory[i].itemCount = 1;
                smith.photonView.RPC("PickUp", RpcTarget.AllViaServer);//�������� ����ٸ� PickUp����
                return;
            }
        }
        Debug.Log("�� ĭ�� �����ϴ�");
        return;
    }

    void UseInventory(int index)
    {
        if (info.Inventory[index].itemCount > 0)
        {
            StartCoroutine(UseItem(info.Inventory[index].itemData, transform.position + effectPos));
            info.Inventory[index].itemCount--;
            if (info.Inventory[index].itemCount <= 0)
            {
                info.Inventory[index].itemData = null;
                PV.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, "T_00", 0);//아이템을 다 썼으니 맨손으로 설정
            }
        }

    }

    IEnumerator UseItem(ItemData useItem, Vector3 usePos)
    {
        info.State = State.Action;
        useItem.Effect(usePos);
        yield return new WaitForSeconds(0.2f);//아이템 데이터에 선딜후딜 저장?
        info.State = State.Idle;
    }

    #endregion

    #region Animation

    [PunRPC]
    void AnimTrigger(string trigger)//�� ĳ������ roll�� ���ư����� ����
    {
        animator.SetTrigger(trigger);
    }

    [PunRPC]
    void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    #endregion

    void SetToolOffset()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        effectPos = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0).normalized;
        toolCollider.offset = effectPos;
    }

    int[] CheckAround(int x, int y)
    {
        int[] result = new int[2];

        for (int tX = -1; tX <= 1; tX++)
        {
            if (x + tX < 0 || x + tX >= tileManager.areaCount * 30) { continue; }
            for (int tY = -1; tY <= 1; tY++)
            {
                if (y + tY < 0 || y + tY >= 14) { continue; }
                if (tileManager.tileInfos[x + tX][y + tY].tileSlot.itemData == null)
                {
                    result[0] = x + tX; result[1] = y + tY;
                    return result;
                }
            }
        }

        return null;
    }
}
