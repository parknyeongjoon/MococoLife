using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
///����Ű�� Dictionary<Keycode, Action> ���� ��� ����� �ִµ� ����ϰ� ���굵 �� ȿ�����̰� ������ ��
public class PlayerMove : MonoBehaviourPun
{
    PhotonView PV;
    TileManager tileManager;
    GameManager gameManager;

    [SerializeField] PlayerInfo info;
    [SerializeField] Animator animator;
    [SerializeField] Collider2D bodyCollider, toolCollider;

    Vector3 moveDir;
    Vector3 effectPos;
    Vector3 mousePos;

    int inventory_Index;

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

            if (Input.GetKeyDown(KeyCode.Space) && moveDir != Vector3.zero)
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
                    info.Hand.itemData.Effect(transform.position + effectPos);
                }
                else if (info.Hand.itemData.Item_Type == Item_Type.BattleItem)
                {
                    UseItem(inventory_Index);
                    
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
                if(info.Inventory[0].itemData != null)
                {
                    inventory_Index = 0;
                    int pX = (int)transform.position.x, pY = (int)transform.position.y;
                    int[] result = CheckAround(pX, pY);
                    if(result == null)
                    {
                        Debug.Log("���� ���� ����");
                    }
                    else
                    {
                        gameManager.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, pX, pY, info.Hand.itemData.code, info.Hand.itemCount);//������ ��������
                        PV.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, info.Inventory[0].itemData.code, info.Inventory[0].itemCount);//�տ��� ���� ���
                    }
                }

            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (info.Inventory[1].itemData != null)
                {
                    inventory_Index = 1;
                    int pX = (int)transform.position.x, pY = (int)transform.position.y;
                    int[] result = CheckAround(pX, pY);
                    if (result == null)
                    {
                        Debug.Log("���� ���� ����");
                    }
                    else
                    {
                        gameManager.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, pX, pY, info.Hand.itemData.code, info.Hand.itemCount);//������ ��������
                        PV.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, info.Inventory[1].itemData.code, info.Inventory[1].itemCount);//�տ��� ���۵��
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (info.Inventory[2].itemData != null)
                {
                    inventory_Index = 2;
                    int pX = (int)transform.position.x, pY = (int)transform.position.y;
                    int[] result = CheckAround(pX, pY);
                    if (result == null)
                    {
                        Debug.Log("���� ���� ����");
                    }
                    else
                    {
                        gameManager.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, pX, pY, info.Hand.itemData.code, info.Hand.itemCount);//������ ��������
                        PV.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, info.Inventory[2].itemData.code, info.Inventory[2].itemCount);//�տ��� ���۵��

                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (info.Inventory[3].itemData != null)
                {
                    inventory_Index = 3;
                    int pX = (int)transform.position.x, pY = (int)transform.position.y;
                    int[] result = CheckAround(pX, pY);
                    if(result == null)
                    {
                        Debug.Log("���� ���� ����");
                    }
                    else
                    {
                        gameManager.photonView.RPC("SetTileItem", RpcTarget.AllViaServer, pX, pY, info.Hand.itemData.code, info.Hand.itemCount);//������ ��������
                        PV.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, info.Inventory[3].itemData.code, info.Inventory[3].itemCount);//�տ��� ���۵��
                    }
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
            animator.SetBool("isMirror", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDir.x = 1;
            animator.SetBool("isMirror", false);

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
        else if(moveDir == Vector3.zero)
        {
            animator.SetBool("isMove", false);
        }
    }

    IEnumerator Dash()
    {
        info.State = State.Dash;

        float time = 0.0f;
        PV.RPC("DashAnim", RpcTarget.AllViaServer);

        while (time <= 0.05f)
        {
            transform.position += moveDir * 15 * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }

        info.State = State.Idle;
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
                if(result == null)//�ֺ��� ���� ���� ���ٸ�
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
    
    void UseItem(int index)
    {
        if(info.Inventory[index].itemCount > 0)
        {
            info.Inventory[index].itemData.Effect(transform.position + effectPos);
            info.Inventory[index].itemCount--;
            if(info.Inventory[index].itemCount <= 0)
            {
                info.Inventory[index].itemData = null;
                PV.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, "T_00", 0);//아이템을 다 썼으니 맨손으로 설정
            }
        }

    }

    #endregion

    #region Animation

    [PunRPC] void DashAnim()//�� ĳ������ roll�� ���ư����� ����
    {
        animator.SetTrigger("roll");
    }

    #endregion

    void SetToolOffset()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        effectPos = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0).normalized;
        toolCollider.offset = effectPos * 0.33f;
    }

    int[] CheckAround(int x, int y)
    {
        int[] result = new int[2];

        for(int tX = -1; tX <= 1; tX++)
        {
            for(int tY = -1; tY <= 1; tY++)
            {
                if(tileManager.tileInfos[x + tX][y + tY].tileSlot.itemData == null)
                {
                    result[0] = x + tX; result[1] = y + tY;
                    return result;
                }
            }
        }

        return null;
    }
}
